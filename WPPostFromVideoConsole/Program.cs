using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using WPPostFromVideoConsole;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Workers;

Env.TraversePath().Load();

var secrets = new[] { Env.GetString("CLIENT_SECRETS_FILE") };

bool forcePublishNow = Env.GetBool("PUBLISH_NOW");

await using var db = new VideoContext();
try
{
    await db.Database.EnsureCreatedAsync();
    await db.Database.MigrateAsync();
}
catch (MySqlException mySqlException)
{
    Console.Write(mySqlException.StackTrace, mySqlException.Message);    
}

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