using WordPressPCL.Models;
using WPPostFromVideoConsole.MediaTypes;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

public interface IWordPress
{
    public Task<MediaItem?> UploadThumbToWp(string url, string file, string id);
    public Task<Post?> CreateNewPost(Video? video, MediaItem mediaItem, PostPublishType postPublishType);
    public Task<IEnumerable<Post>> GetPosts();
    public Task<Post> GetPostById(int id);
    public Task<string> GetMediaUrlById(int? id);
}