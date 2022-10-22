using System.Diagnostics.CodeAnalysis;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Interfaces;

[SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
public interface IDb
{
    public Video? GetVideoFromDb(VideoContext context);
    public List<Post> GetPostsFromDb(VideoContext context);
    public Video? GetVideoFromDbById(VideoContext context, string id);
    public int AddVideoToDb(VideoContext context, Video? video);
    public void PutPostInDb(VideoContext ctx, Post post);

    // ReSharper disable once UnusedMember.Global
    public void EditVideoInDb(VideoContext ctx, Video video);

    // ReSharper disable once UnusedMember.Global
    public void EditPostInDb(VideoContext ctx, Post post);
}