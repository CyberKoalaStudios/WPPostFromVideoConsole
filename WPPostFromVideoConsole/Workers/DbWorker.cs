using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Interfaces;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class DbWorker : IDb
{
    public static DbWorker Instance = new();

    public Video? GetVideoFromDb(VideoContext context)
    {
        var videoFromDb = context.Videos
            .OrderBy(b => b.PublishedAt)
            .FirstOrDefault();

        return videoFromDb;
    }

    public List<Post> GetPostsFromDb(VideoContext context)
    {
        var videoFromDb = context.Posts
            .OrderBy(b => b.Timestamp).ToList();

        return videoFromDb;
    }
    public Video? GetVideoFromDbById(VideoContext context, string id)
    {
        var videoFromDb = context.Videos
            .Where(a => a.Id.Equals(id))
            .OrderBy(b => b.PublishedAt)
            .FirstOrDefault();

        return videoFromDb;
    }

    public int AddVideoToDb( VideoContext context, Video? video)
    {
        if (video == null) return -1;

        video.IsPublished = true;

        Console.WriteLine("Inserting a new video in DB");
        context.Add(video);
        var stateEntitiesWritten = context.SaveChanges();
        return stateEntitiesWritten;
    }

    public void PutPostInDb(VideoContext ctx, Post post)
    {
        ctx.Add(post);
        ctx.SaveChanges();
    }
    
    public void PutPostWithVideoInDb(VideoContext ctx, Post post, Video video)
    {
        post.Video = video;
        post.VideoIdx = video.Idx;
        ctx.Posts.Add(post);
        var num = ctx.SaveChanges();
    }

    public void EditVideoInDb(VideoContext context, Video video)
    {
        context.Videos.Update(video);
        context.SaveChanges();
    }    
    
    public void EditPostInDb(VideoContext context, Post post)
    {
        context.Posts.Update(post);
        context.SaveChanges();
    }
}