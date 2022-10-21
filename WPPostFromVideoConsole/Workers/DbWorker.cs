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

    public Video? GetVideoFromDbById(VideoContext context, string id)
    {
        var videoFromDb = context.Videos
            .Where(a => a.Id.Equals(id))
            .OrderBy(b => b.PublishedAt)
            .FirstOrDefault();

        return videoFromDb;
    }

    public int AddVideoToDb(Video? video, VideoContext context)
    {
        if (video == null) return -1;

        video.IsPublished = true;

        Console.WriteLine("Inserting a new video in DB");
        context.Add(video);
        var stateEntitiesWritten = context.SaveChanges();
        return stateEntitiesWritten;
    }
}