using System.Net;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.EntityFrameworkCore;
using WordPressPCL;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Interfaces;
using WPPostFromVideoConsole.Models;
using WPPostFromVideoConsole.Workers;

namespace WPPostFromVideoConsole;


public enum PostPublishType
{
    Scheduled,
    Now
}

/// <summary>
/// YouTube Data API v3 sample: retrieve my uploads.
/// Relies on the Google APIs Client Library for .NET, v1.7.0 or higher.
/// See https://developers.google.com/api-client-library/dotnet/get_started
/// </summary>
internal class MyUploads
{
    //pass the Wordpress REST API base address as string
    WordPressClient _wordPressClient = new WordPressClient(System.Environment.GetEnvironmentVariable("WP_REST_URI"));

    private PostPublishType _postPublishType = PostPublishType.Scheduled;
        
    [STAThread]
    public static void GetUploads(string[] args)
    {
        DotNetEnv.Env.TraversePath().Load();
        Console.WriteLine("YouTube Data API: My Uploads");
        Console.WriteLine("============================");

        try
        {
            foreach (var arg in args)
            {
                new MyUploads().Run(arg).Wait();
            }
        }
        catch (AggregateException ex)
        {
            foreach (var e in ex.InnerExceptions)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }

    private async Task Run(string clientSecretsFile)
    {
        UserCredential credential;
        
        await using var db = new VideoContext();
        db.Database.Migrate();
        
        await using(var stream = new System.IO.FileStream(clientSecretsFile, FileMode.Open, FileAccess.Read))
        {
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                // user's account, but not other types of account access.
                new[] { YouTubeService.Scope.YoutubeReadonly },
                "user",
                CancellationToken.None,
                new FileDataStore(this.GetType().ToString())
            );
        }

        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = this.GetType().ToString()
        });

        var channelsListRequest = youtubeService.Channels.List("contentDetails");
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
                var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                playlistItemsListRequest.PlaylistId = uploadsListId;
                playlistItemsListRequest.MaxResults = 5;
                playlistItemsListRequest.PageToken = nextPageToken;

                // Retrieve the list of videos uploaded to the authenticated user's channel.
                var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                foreach (var playlistItem in playlistItemsListResponse.Items)
                {
                    // Print information about each video.
                    Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title, playlistItem.Snippet.ResourceId.VideoId);
                    Console.WriteLine($"Published: {playlistItem.Snippet.PublishedAt.Value}");
                }

                var latestItem = playlistItemsListResponse.Items.First();
                
                var videoFromDb = DbWorker.Instance.GetVideoFromDbById(db, latestItem.Snippet.ResourceId.VideoId);
                
                if (videoFromDb?.Id == latestItem.Snippet.ResourceId.VideoId) continue;
                // if (videoFromDb.IsPublished) continue;
                
                var video = new Video()
                {
                    Id = latestItem.Snippet.ResourceId.VideoId,
                    Description = latestItem.Snippet.Description,
                    PublishedAt = latestItem.Snippet.PublishedAt.Value,
                    Title = latestItem.Snippet.Title,
                    Thumbnail = latestItem.Snippet.Thumbnails.Maxres.Url,
                    IsPublished = false
                };
                
                var createdMedia =  await WordPressWorker.Instance.UploadThumbToWp(video.Thumbnail, "preview.jpg", video.Id);
                
                var createdPost = await WordPressWorker.Instance.CreateNewPost(video, createdMedia, _postPublishType);

                var addedToDb =  DbWorker.Instance.AddVideoToDb(video, db);

                nextPageToken = playlistItemsListResponse.NextPageToken;
            // }
        }
    }
}