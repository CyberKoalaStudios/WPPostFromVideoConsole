using System.Net;
using System.Reflection.Metadata;
using WordPressPCL;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Helpers;

public class VideoWorker: IVideo
{
    public static VideoWorker Instance = new VideoWorker();
    
    readonly WordPressClient _wordPressClient = new WordPressClient(System.Environment.GetEnvironmentVariable("WP_REST_URI"));

    public Video? GetVideoFromDb(VideoContext context)
    {
        var videoFromDb = context.Videos
            .OrderBy(b => b.PublishedAt)
            .FirstOrDefault();

        return videoFromDb;
    }

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
        // returns created post
        var post = new Post()
        {
            Title = new Title(video?.Title),
            Content = new Content($"\n{video?.Description}"),
            //Author = 1,
            Status = Status.Future,
            Date = DateTime.Now.AddDays(1), // video.publishedAt.AddMinutes(10)
            Categories = new List<int>(){81},
            //Format = "standart",
            CommentStatus = OpenStatus.Open,
            FeaturedMedia = createdMedia?.Id
        };
                
        var createdPost = await _wordPressClient.Posts.CreateAsync(post);
        return createdPost;
    }

    public void AddVideoToDb(Video? video, VideoContext context)
    {
        video.IsPublished = true;
                
        Console.WriteLine("Inserting a new video in DB");
        context.Add(video);
        context.SaveChanges();
    }
}