using DotNetEnv;
using WordPressPCL.Models;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class PostWorker
{
    private readonly DiscordSender _discordSender = new(Env.GetString("DISCORD_TOKEN"));
    private readonly TelegramSender _telegramSender = new(Env.GetString("TELEGRAM_BOT_TOKEN"));

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
                
                break;
            }
        }
    }

    private void OnVideoPublished(Video video)
    {
        var discordPost = _discordSender.CreateFromYouTube(video);
        var telegramPost = _telegramSender.CreateFromYouTube(video);
    }
}