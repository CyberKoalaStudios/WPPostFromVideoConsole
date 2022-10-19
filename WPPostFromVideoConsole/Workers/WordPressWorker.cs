using System.Net;
using WordPressPCL;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Interfaces;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class WordPressWorker : IWordPress
{
    public static WordPressWorker Instance = new();

    private readonly WordPressClient _wordPressClient = new(Environment.GetEnvironmentVariable("WP_REST_URI"));

    [Obsolete("Obsolete")]
    public async Task<MediaItem?> UploadThumbToWp(string url, string file, string id)
    {
        // Download Thumb
        const string thumbFile = "preview.jpg";

        using var client = new WebClient();
        client.DownloadFile(new Uri(url), file);

        _wordPressClient.Auth.UseBasicAuth(DotNetEnv.Env.GetString("WP_USERNAME"),
            DotNetEnv.Env.GetString("WP_APP_PASSWORD"));

        Console.WriteLine("Uploading Thumbnail into WordPress");
        var createdMedia = await _wordPressClient.Media.CreateAsync(thumbFile, $"thumb_{id}.jpg");

        return createdMedia;
    }

    public async Task<Post?> CreateNewPost(Video? video, MediaItem? createdMedia, PostPublishType postPublishType)
    {
        var iframe =
            $"<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/{video?.Id}\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";

        int delayToAdd = 0;
        if (postPublishType == PostPublishType.Scheduled)
        {
            delayToAdd = DotNetEnv.Env.GetInt("POST_DELAY");
        }

        var post = new Post()
        {
            Title = new Title(video?.Title),
            Content = new Content($"{iframe}\n{video?.Description}"),
            Status = Status.Future,
            Date = DateTime.Now.AddDays(delayToAdd),
            Categories = new List<int>() { DotNetEnv.Env.GetInt("POST_CATEGORY") },
            CommentStatus = OpenStatus.Open,
            FeaturedMedia = createdMedia?.Id
        };

        Console.WriteLine("Creating Post...");
        var createdPost = await _wordPressClient.Posts.CreateAsync(post);
        return createdPost;
    }


}