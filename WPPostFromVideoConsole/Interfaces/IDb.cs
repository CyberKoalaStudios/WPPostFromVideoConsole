using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

public interface IDb
{
    public Video? GetVideoFromDb(VideoContext context);
    public Video? GetVideoFromDbById(VideoContext context, string id);
    public int AddVideoToDb(Video? video, VideoContext context);
}