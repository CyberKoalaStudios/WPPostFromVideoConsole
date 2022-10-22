using DotNetEnv;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Helpers;
using WPPostFromVideoConsole.Models;
using Post = WPPostFromVideoConsole.Models.Post;

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

    private void OnPostPublished(WordPressPCL.Models.Post post)
    {
        switch (post.Status)
        {
            case Status.Publish:
            {
                _discordSender.CreateFromWordPress(post);
                _telegramSender.CreateFromWordPress(post);
                
                break;
            }
            case Status.Future:
            case Status.Private:
            case Status.Pending:
            {
                // TODO: Add scheduler to discord/telegram posts, publish onto {post.Date}
                PutPostInDb(post);
                break;
            }
        }
    }   
    private void OnPostWithVideoPublished(WordPressPCL.Models.Post post, Video video)
    {
        switch (post.Status)
        {
            case Status.Publish:
            {
                _discordSender.CreateFromWordPress(post);
                 _telegramSender.CreateFromWordPress(post);
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

    private void PutPostInDb(WordPressPCL.Models.Post post)
    {
        var postParams = new Post
        {
            PostName = post.Title.Rendered,
            Description = Formatter.StripHtml(post.Content.Rendered),
            Url = post.Link,
            ImageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result,
            Timestamp = post.Date,
            WordpressId = post.Id
        };

        Mappings.PostToDb.postStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.Status = _postStatus;
        
        using var db = new VideoContext();
        DbWorker.Instance.PutPostInDb(db, postParams);
        // TODO: Create service that checks status of post -> if published -> Update In DB and publish to discord/Telegram
    }
    
    private void PutPostWithVideoInDb(WordPressPCL.Models.Post post, Video video)
    {
        var postParams = new Post
        {
            PostName = post.Title.Rendered,
            Description = Formatter.StripHtml(post.Content.Rendered),
            Url = post.Link,
            ImageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result,
            Timestamp = post.Date,
            WordpressId = post.Id,
        };

        Mappings.PostToDb.postStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.Status = _postStatus;
        
        using var db = new VideoContext();
        DbWorker.Instance.PutPostWithVideoInDb(db, postParams, video);
    }
    
}