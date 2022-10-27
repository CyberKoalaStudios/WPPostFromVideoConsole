using DotNetEnv;
using WPPostFromVideoConsole;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Workers;

Env.TraversePath().Load();

var secrets = new[] { Env.GetString("CLIENT_SECRETS_FILE") };

const bool forcePublishNow = true;
if (forcePublishNow)
{
    PublishNowLatestPostFromWp();
}
else
{
    var publisher = new Publisher();
    await publisher.StatusChecker();

    MyUploads.GetUploads(secrets);
}

void PublishNowLatestPostFromWp()
{
    DiscordSender _discordSender = new(Env.GetString("DISCORD_TOKEN"));
    TelegramSender _telegramSender = new(Env.GetString("TELEGRAM_BOT_TOKEN"));
    
    var post = WordPressWorker.Instance.GetLatestPost().Result;
    if (post == null) return;
    _discordSender.CreateFromWordPress(post);
    _telegramSender.CreateFromWordPress(post);
}