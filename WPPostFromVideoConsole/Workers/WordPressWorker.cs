using DotNetEnv;
using WordPressPCL;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Helpers;
using WPPostFromVideoConsole.Interfaces;
using WPPostFromVideoConsole.MediaTypes;
using WPPostFromVideoConsole.Models;
using Post = WordPressPCL.Models.Post;

namespace WPPostFromVideoConsole.Workers;

public class WordPressWorker : IWordPress
{
    public static readonly WordPressWorker Instance = new();
    private readonly string _appPassword = Env.GetString("WP_APP_PASSWORD");
    private readonly int _postCategory = Env.GetInt("POST_CATEGORY");
    private readonly int _postDelay = Env.GetInt("POST_DELAY");
    private readonly int _publishHour = Env.GetInt("PUBLISH_HOUR");
    private readonly string _username = Env.GetString("WP_USERNAME");

    private WordPressClient _wordPressClient = new(Environment.GetEnvironmentVariable("WP_REST_URI"));

    private WordPressWorker()
    {
        // _wordPressClient.Auth.UseBearerAuth(JWTPlugin.JWTAuthByEnriqueChavez);
        // await _wordPressClient.Auth.RequestJWTokenAsync("username", "password");
        // var isValidToken = await _wordPressClient.IsValidJWTokenAsync();
        
        _wordPressClient.Auth.UseBasicAuth(_username, _appPassword);
    }
    
    public async Task<MediaItem?> UploadThumbToWp(string url, string file, string id)
    {
        const string thumbFile = "preview.jpg";

        HttpHelper.DownloadFileAsync(url, file);

        Console.WriteLine("Uploading Thumbnail into WordPress");
        var createdMedia = await _wordPressClient.Media.CreateAsync(thumbFile, $"thumb_{id}.jpg");

        return createdMedia;
    }

    public async Task<Post?> CreateNewPost(Video? video, MediaItem? createdMedia, PostPublishType postPublishType)
    {
        var iframe =
            $"<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/{video?.Id}\" " +
            "title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; " +
            // ReSharper disable once StringLiteralTypo
            "encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";

        var publishDate = DateTime.Now;

        if (postPublishType == PostPublishType.Scheduled)
            publishDate = DateTime.Today.AddDays(_postDelay).AddHours(_publishHour);

        var post = new Post
        {
            Title = new Title(video?.Title),
            Content = new Content($"{iframe}\n{video?.Description}"),
            Status = Status.Future,
            Date = publishDate,
            Categories = new List<int> { _postCategory },
            CommentStatus = OpenStatus.Open,
            FeaturedMedia = createdMedia?.Id
        };

        Console.WriteLine("Creating Post...");
        var createdPost = await _wordPressClient.Posts.CreateAsync(post);
        return createdPost;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        return await _wordPressClient.Posts.GetAllAsync();
    }

    public async Task<Post> GetPostById(int postId)
    {
        return await _wordPressClient.Posts.GetByIDAsync(postId);
    }

    public async Task<string> GetMediaUrlById(int? mediaId)
    {
        var media = await _wordPressClient.Media.GetByIDAsync(mediaId);
        return media.Link;
    }
}