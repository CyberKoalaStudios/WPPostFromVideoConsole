using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Mappings;
using WPPostFromVideoConsole.Workers;

namespace WPPostFromVideoConsole;

public class Publisher
{
    private PostWorker _postWorker;

    public Publisher()
    {
        _postWorker = new PostWorker();
    }

    public static event Action<Post> PostStatusChanged = delegate { };

    public async Task StatusChecker()
    {
        await using var context = new VideoContext();

        var postsFromDb = DbWorker.Instance.GetPostsFromDb(context);
        foreach (var post in postsFromDb)
        {
            var reversed = PostToDb.PostStatusMap.ToDictionary(x => x.Value, x => x.Key);
            reversed.TryGetValue(post.Status, out var statusFromInnerDb);
            
            var wpPost = await WordPressWorker.Instance.GetPostById(post.WordpressId);
            
            if (wpPost.Status == statusFromInnerDb) continue;
            
            PostToDb.PostStatusMap.TryGetValue(wpPost.Status, out var newStatus);

            // If Status = Publish
            post.Status = newStatus;
            post.Timestamp = DateTimeOffset.Now;

            // Send to Tg, Discord
            PostStatusChanged?.Invoke(wpPost);
            
            // Update attached video status: TODO: handle case if YouTube Status updated, but WP is not
            var attachedVideoIndex = post.VideoIdx;
            var video = DbWorker.Instance.GetVideoFromDbByIdx(context, attachedVideoIndex);
            video.IsPublished = true;
            post.Video = video;
            
            context.Update(post);
            context.SaveChanges();
        }
    }
}