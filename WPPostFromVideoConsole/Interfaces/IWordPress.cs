using WordPressPCL.Models;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

public interface IWordPress
{
    public Task<MediaItem?> UploadThumbToWp(string url, string file, string id);
    public Task<Post?> CreateNewPost(Video? video, MediaItem mediaItem);
}