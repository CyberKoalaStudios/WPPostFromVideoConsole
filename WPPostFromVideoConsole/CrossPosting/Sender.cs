using Discord.Webhook;
using DotNetEnv;
using Telegram.Bot;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Models;
using Video = WPPostFromVideoConsole.Models.Video;

namespace WPPostFromVideoConsole.CrossPosting;

internal abstract class Sender
{
    public Sender(string token)
    {
        Token = token;
    }

    public string Token { get; set; }

    public abstract MessengerPost CreateFromYouTube(Video video);
    public abstract MessengerPost CreateFromWordPress(Post post);
}

#region Senders

internal class TelegramSender : Sender
{
    private TelegramBotClient botClient;
    public TelegramSender(string token) : base(token)
    {
        botClient = new TelegramBotClient(token);
    }

    public override MessengerPost CreateFromYouTube(Video video)
    {
        return new Telegram(botClient, video);
    }
    
    public override MessengerPost CreateFromWordPress(Post post)
    {
        return new Telegram(botClient, post);
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