using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Helpers;

public interface IVideo
{
    public Video? GetVideoFromDb(VideoContext context);
    public Task<MediaItem?> UploadThumbToWp(string url, string file, string id);
    public Task<Post?> CreateNewPost(Video? video, MediaItem mediaItem);
    public void AddVideoToDb(Video? video, VideoContext context);
}