using Discord.Webhook;
using DotNetEnv;
using Telegram.Bot;
using WPPostFromVideoConsole.Models;
using Post = WordPressPCL.Models.Post;

namespace WPPostFromVideoConsole.CrossPosting;

internal abstract class Sender
{
    // ReSharper disable once UnusedParameter.Local
    protected Sender(string token)
    {
    }

    // ReSharper disable once UnusedMemberInSuper.Global
    public abstract MessengerPost CreateFromYouTube(Video video);

    // ReSharper disable once UnusedMemberInSuper.Global
    public abstract MessengerPost CreateFromWordPress(Post post);
}

#region Senders

internal class TelegramSender : Sender
{
    private readonly TelegramBotClient _botClient;

    public TelegramSender(string token) : base(token)
    {
        _botClient = new TelegramBotClient(token);
    }

    public override MessengerPost CreateFromYouTube(Video video)
    {
        return new Telegram(_botClient, video);
    }

    public override MessengerPost CreateFromWordPress(Post post)
    {
        return new Telegram(_botClient, post);
    }
}

internal class DiscordSender : Sender
{
    private readonly DiscordWebhookClient _client;
    private readonly string _id = Env.GetString("DISCORD_ID");

    public DiscordSender(string token) : base(token)
    {
        var webHookUrl = $"https://discord.com/api/webhooks/{_id}/{token}";
        _client = new DiscordWebhookClient(webHookUrl);
    }

    public override MessengerPost CreateFromYouTube(Video video)
    {
        return new Discord(_client, video);
    }

    public override MessengerPost CreateFromWordPress(Post post)
    {
        return new Discord(_client, post);
    }
}

#endregion