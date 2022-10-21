using DotNetEnv;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Helpers;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class PostWorker
{

    private readonly DiscordSender _discordSender = new(Env.GetString("DISCORD_TOKEN"));
    private readonly TelegramSender _telegramSender = new(Env.GetString("TELEGRAM_BOT_TOKEN"));


    private byte _postStatus;
    
    public PostWorker()
    {
        MyUploads.PostPublishedInDb += OnPostPublished;
        MyUploads.VideoPublished += OnVideoPublished;
        MyUploads.PostAndVideoPublishedInDb += OnPostWithVideoPublished;
    }

    private void OnPostPublished(Post post)
    {
        switch (post.Status)
        {
            case Status.Publish:
            {
                var discordPost = _discordSender.CreateFromWordPress(post);
                var telegramPost = _telegramSender.CreateFromWordPress(post);
                break;
            }
            case Status.Future:
            //case Status.Private:
            {
                // TODO: Add scheduler to discord/telegram posts, publish onto {post.Date}
                PutPostInDb(post);
                break;
            }
        }
    }   
    private void OnPostWithVideoPublished(Post post, Video video)
    {
        switch (post.Status)
        {
            case Status.Publish:
            {
                var discordPost = _discordSender.CreateFromWordPress(post);
                var telegramPost = _telegramSender.CreateFromWordPress(post);
                break;
            }
            case Status.Future:
            //case Status.Private:
            {
                // TODO: Add scheduler to discord/telegram posts, publish onto {post.Date}
                PutPostWithVideoInDb(post, video);
                break;
            }
        }
    }

    private void OnVideoPublished(Video video)
    {
        var discordPost = _discordSender.CreateFromYouTube(video);
        var telegramPost = _telegramSender.CreateFromYouTube(video);
    }

    private void PutPostInDb(Post post)
    {
        var postParams = new PostParams();
        postParams.postName = post.Title.Rendered;
        postParams.description = Formatter.StripHtml(post.Content.Rendered);
        postParams.url = post.Link;
        postParams.imageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result;
        postParams.timestamp = post.Date;

        Mappings.PostToDb.postStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.status = _postStatus;
        
        using var db = new VideoContext();
        DbWorker.Instance.PutPostInDb(db, postParams);
        
        
    }
    private void PutPostWithVideoInDb(Post post, Video video)
    {
        var postParams = new PostParams();
        postParams.postName = post.Title.Rendered;
        postParams.description = Formatter.StripHtml(post.Content.Rendered);
        postParams.url = post.Link;
        postParams.imageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result;
        postParams.timestamp = post.Date;

        Mappings.PostToDb.postStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.status = _postStatus;
        
        using var db = new VideoContext();
        DbWorker.Instance.PutPostWithVideoInDb(db, postParams, video);
    }
    
}