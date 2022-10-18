using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

public interface IDb
{
    public Video? GetVideoFromDb(VideoContext context);
    public int AddVideoToDb(Video? video, VideoContext context);
}