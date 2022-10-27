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

            if (statusFromInnerDb is not Status.Publish) continue;
            
            var wpPost = await WordPressWorker.Instance.GetPostById(post.WordpressId);
            
            if (wpPost.Status == statusFromInnerDb) continue;
            
            // TODO: Debug
            PostToDb.PostStatusMap.TryGetValue(wpPost.Status, out var newStatus);

            // If Status = Publish
            post.Status = newStatus;
            post.Timestamp = DateTimeOffset.Now;

            // Send to Tg, Discord
            PostStatusChanged?.Invoke(wpPost);

            context.Update(post);

            await context.SaveChangesAsync();
        }
    }
}