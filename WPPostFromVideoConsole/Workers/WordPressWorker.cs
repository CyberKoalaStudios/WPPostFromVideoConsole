using System.Net;
using WordPressPCL;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Interfaces;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class WordPressWorker: IWordPress
{
    public static WordPressWorker Instance = new WordPressWorker();
    
    readonly WordPressClient _wordPressClient = new WordPressClient(System.Environment.GetEnvironmentVariable("WP_REST_URI"));

    public async Task<MediaItem?> UploadThumbToWp(string url, string file, string id)
    {
        // Download Thumb
        string thumbFile = "preview.jpg";

        using WebClient client = new WebClient();
        client.DownloadFile(new Uri(url), file);
        // OR 
        //client.DownloadFileAsync(new System.Uri(video.Thumbnail), thumbFile);
        
        _wordPressClient.Auth.UseBasicAuth( DotNetEnv.Env.GetString("WP_USERNAME"), 
            DotNetEnv.Env.GetString("WP_APP_PASSWORD"));

        Console.WriteLine("Uploading Thumbnail into WordPress");
        var createdMedia = await _wordPressClient.Media.CreateAsync(thumbFile,$"thumb_{id}.jpg");

        //throw NotImplementedException;
        return createdMedia;
    }

    public async Task<Post?> CreateNewPost(Video? video, MediaItem? createdMedia)
    {
        var iframe =
            $"<iframe width=\"560\" height=\"315\" src=\"https://www.youtube.com/embed/{video?.Id}\" title=\"YouTube video player\" frameborder=\"0\" allow=\"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture\" allowfullscreen></iframe>";
        
        var post = new Post()
        {
            Title = new Title(video?.Title),
            Content = new Content($"{iframe}\n{video?.Description}"),
            //Author = 1,
            Status = Status.Future,
            Date = DateTime.Now.AddDays(1), // video.publishedAt.AddMinutes(10)
            Categories = new List<int>(){81},
            //Format = "standart",
            CommentStatus = OpenStatus.Open,
            FeaturedMedia = createdMedia?.Id
        };
                
        Console.WriteLine("Creating Post WordPress");
        var createdPost = await _wordPressClient.Posts.CreateAsync(post);
        return createdPost;
    }
    
}