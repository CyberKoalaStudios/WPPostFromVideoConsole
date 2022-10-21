using DotNetEnv;
using WordPressPCL.Models;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Workers;

public class PostWorker
{
    private readonly DiscordSender _discordSender = new(Env.GetString("DISCORD_TOKEN"));
    private readonly TelegramSender _telegramSender = new("Telegram Token");

    public PostWorker()
    {
        MyUploads.PostPublishedInDb += OnPostPublished;
        MyUploads.VideoPublished += OnVideoPublished;
    }

    private void OnPostPublished(Post post)
    {
        var discordPost = _discordSender.CreateFromWordPress(post);
        var telegramPost = _telegramSender.CreateFromWordPress(post);
    }

    private void OnVideoPublished(Video video)
    {
        var discordPost = _discordSender.CreateFromYouTube(video);
        var telegramPost = _telegramSender.CreateFromYouTube(video);
    }
}