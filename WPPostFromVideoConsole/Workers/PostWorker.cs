using DotNetEnv;
using WordPressPCL.Models;
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
                PutPostInMq(post);
                break;
            }
        }
    }

    private void OnVideoPublished(Video video)
    {
        var discordPost = _discordSender.CreateFromYouTube(video);
        var telegramPost = _telegramSender.CreateFromYouTube(video);
    }

    private void PutPostInMq(Post post)
    {
        var _postParams = new PostParams();
        _postParams.postName = post.Title.Rendered;
        _postParams.description = Formatter.StripHtml(post.Content.Rendered);
        _postParams.url = post.Link;
        _postParams.imageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result;
        _postParams.timestamp = post.Date;

        Mappings.PostToDb.postStatusMap.TryGetValue(post.Status, out _postStatus);
        _postParams.status = _postStatus;
        
        
    }
    
}