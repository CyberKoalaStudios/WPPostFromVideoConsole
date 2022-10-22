using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

public interface IDb
{
    public Video? GetVideoFromDb(VideoContext context);
    public Video? GetVideoFromDbById(VideoContext context, string id);
    public int AddVideoToDb(VideoContext context, Video? video);
    public void PutPostInDb(VideoContext ctx, PostParams post);
    public void EditVideoInDb(VideoContext ctx, Video video);
}