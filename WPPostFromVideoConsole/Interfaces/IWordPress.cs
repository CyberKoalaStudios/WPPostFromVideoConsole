using System.Diagnostics.CodeAnalysis;
using WordPressPCL.Models;
using WPPostFromVideoConsole.MediaTypes;
using WPPostFromVideoConsole.Models;
using Post = WordPressPCL.Models.Post;

namespace WPPostFromVideoConsole.Interfaces;

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IWordPress
{
    public Task<MediaItem?> UploadThumbToWp(string url, string id);
    public Task<Post?> CreateNewPost(Video? video, MediaItem mediaItem, PostPublishType postPublishType);

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once UnusedMember.Global
    public Task<IEnumerable<Post>> GetPosts();
    public Task<Post> GetPostById(int id);
    public Task<string> GetMediaUrlById(int? id);
}