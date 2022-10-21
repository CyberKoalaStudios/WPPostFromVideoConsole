using DotNetEnv;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.EntityFrameworkCore;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.MediaTypes;
using WPPostFromVideoConsole.Models;
using WPPostFromVideoConsole.Workers;

namespace WPPostFromVideoConsole;

/// <summary>
///     YouTube Data API v3 sample: retrieve my uploads.
///     Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
///     See https://developers.google.com/api-client-library/dotnet/get_started
/// </summary>
internal class MyUploads
{
    private readonly int _mode = Env.GetInt("MODE");

    private PostPublishType _postPublishType;
    private VideoStatus _videoStatus;


    private PostWorker _postWorker = new(); // Subscribe to delegate on init
    public static event Action<Post> PostPublishedInDb = delegate { };
    public static event Action<Post, Video> PostAndVideoPublishedInDb = delegate { };
    public static event Action<WPPostFromVideoConsole.Models.Video> VideoPublished = delegate { };

    [STAThread]
    public static void GetUploads(string[] args)
    {
        Env.TraversePath().Load();
        Console.WriteLine("YouTube Data API: My Uploads");
        Console.WriteLine("============================");

        try
        {
            foreach (var arg in args) new MyUploads().Run(arg).Wait();
        }
        catch (AggregateException ex)
        {
            foreach (var e in ex.InnerExceptions) Console.WriteLine("Error: " + e.Message);
        }
    }

    private async Task Run(string clientSecretsFile)
    {
        UserCredential credential;

        await using var db = new VideoContext();
        db.Database.Migrate();

        await using (var stream = new FileStream(clientSecretsFile, FileMode.Open, FileAccess.Read))
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                // user's account, but not other types of account access.
                new[] { YouTubeService.Scope.YoutubeReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore(GetType().ToString())
            );
        }

        var youtubeService = new YouTubeService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = GetType().ToString()
        });

        var channelsListRequest = youtubeService.Channels.List("contentDetails, status");
        channelsListRequest.Mine = true;

        // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
        var channelsListResponse = await channelsListRequest.ExecuteAsync();

        foreach (var channel in channelsListResponse.Items)
        {
            // From the API response, extract the playlist ID that identifies the list
            // of videos uploaded to the authenticated user's channel.
            var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;

            Console.WriteLine("Videos in list {0}", uploadsListId);

            var nextPageToken = "";
            // while (nextPageToken != null)
            // {
            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet, status");
            playlistItemsListRequest.PlaylistId = uploadsListId;
            playlistItemsListRequest.MaxResults = 2;
            playlistItemsListRequest.PageToken = nextPageToken;

            // Retrieve the list of videos uploaded to the authenticated user's channel.
            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

            foreach (var playlistItem in playlistItemsListResponse.Items)
            {
                // Print information about each video.
                Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
                Console.WriteLine($"Published: {playlistItem.Snippet.PublishedAt}");

                var status = playlistItem.Status.PrivacyStatus;
                Mappings.VideoToSite.videoStatusMap.TryGetValue(status, out _videoStatus);

                Mappings.VideoToSite.videoStatusToPostMap.TryGetValue(_videoStatus, out _postPublishType);
#if DEBUG

                var mockVideo = new Video
                {
                    Id = playlistItem.Snippet.ResourceId.VideoId,
                    Description = playlistItem.Snippet.Description,
                    PublishedAt = playlistItem.Snippet.PublishedAt,
                    Title = playlistItem.Snippet.Title,
                    Thumbnail = playlistItem.Snippet.Thumbnails.Maxres.Url,
                    IsPublished = false
                };
                var testPost = await WordPressWorker.Instance.GetPostById(9094);
                testPost.Status = Status.Future;
               
                //PostPublishedInDb?.Invoke(testPost);
                //var createdMediaTest = await WordPressWorker.Instance.UploadThumbToWp(mockVideo.Thumbnail, "preview.jpg", mockVideo.Id);

                //VideoPublished?.Invoke(mockVideo);

                //PostPublishedInDb?.Invoke(testPost);
                PostAndVideoPublishedInDb?.Invoke(testPost, mockVideo);
                // END TEST
#endif

                var videoFromDb = DbWorker.Instance.GetVideoFromDbById(db, playlistItem.Snippet.ResourceId.VideoId);

                if (videoFromDb?.Id == playlistItem.Snippet.ResourceId.VideoId) continue;

                var video = new Video
                {
                    Id = playlistItem.Snippet.ResourceId.VideoId,
                    Description = playlistItem.Snippet.Description,
                    PublishedAt = playlistItem.Snippet.PublishedAt,
                    Title = playlistItem.Snippet.Title,
                    Thumbnail = playlistItem.Snippet.Thumbnails.Maxres.Url,
                    IsPublished = false
                };

                var createdMedia =
                    await WordPressWorker.Instance.UploadThumbToWp(video.Thumbnail, "preview.jpg", video.Id);
                var createdPost = await WordPressWorker.Instance.CreateNewPost(video, createdMedia, _postPublishType);
                var addedToDb = DbWorker.Instance.AddVideoToDb(db, video);

                if (addedToDb != -1 && createdPost != null)
                {
                    //PostAndVideoPublishedInDb?.Invoke(createdPost, video);
                    PostPublishedInDb?.Invoke(createdPost);
                }
            }

            //var latestItem = playlistItemsListResponse.Items.First();

            nextPageToken = playlistItemsListResponse.NextPageToken;
            // }
        }
    }
}