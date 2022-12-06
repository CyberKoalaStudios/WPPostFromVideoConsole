using Discord;
using Discord.Webhook;
using DotNetEnv;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using WPPostFromVideoConsole.Helpers;
using WPPostFromVideoConsole.Mappings;
using WPPostFromVideoConsole.Workers;
using Post = WordPressPCL.Models.Post;
using Video = WPPostFromVideoConsole.Models.Video;

namespace WPPostFromVideoConsole.CrossPosting;

internal abstract class MessengerPost
{
}

#region Messengers


internal class Telegram : MessengerPost
{
    private readonly string _authorNameRu = Env.GetString("AUTHOR_NAME_RU");
    private readonly string _rhash = Env.GetString("TELEGRAM_RHASH");
    private readonly TelegramBotClient _botClient;
    private long _chatId = Convert.ToInt64(Env.GetString("TELEGRAM_CHAT_ID"));
    private int _telegramPostMode = Env.GetInt("TELEGRAM_POST_MODE");
    private readonly Post _post;
    private Video? _video;

    public Telegram(TelegramBotClient botClient, Post post)
    {
        _botClient = botClient;
        _post = post;
        
        PublishNow().GetAwaiter().GetResult();

        Console.WriteLine("Telegram post sent");
    }


    public Telegram(TelegramBotClient botClient, Video video)
    {
        _botClient = botClient;
        _video = video;

        throw new NotImplementedException();
    }

    private async Task PublishNow()
    {
        // ReSharper disable once StringLiteralTypo
        var caption = $"<b>{Formatter.StripHtml(_post.Title.Rendered)}</b>\n" +
                      $"{Formatter.StripHtml(_post.Content.Rendered)}\n" +
                      $"<i>Источник: </i><a href=\"{Formatter.StripHtml(_post.Link)}\">{_authorNameRu}</a>";

        TelegramPostType.TelegramPostMap.TryGetValue((byte)_telegramPostMode, out var tgPostMode);
        
        switch (tgPostMode)
        {
            case TelegramPostMode.InstantView:
                caption = _post.Title.Rendered;

                //TODO: DEBUG
                var removedLastSlash = _post.Link.Remove(_post.Link.LastIndexOf('/'));

                await _botClient.SendTextMessageAsync(
                    new ChatId(_chatId),
                    caption + "\n" +
                    $"https://t.me/iv?url={_post.Link}&rhash={_rhash}"
                );
                break;
            case TelegramPostMode.InlineButton:

                var photoSrc = WordPressWorker.Instance.GetMediaUrlById(_post?.FeaturedMedia ?? 6762).Result;
                Message message = await _botClient.SendPhotoAsync(
                    chatId: new ChatId(_chatId),
                    photo: photoSrc,
                    caption: caption,
                    replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                        "Посмотреть",
                        _post.Link)),
                    parseMode: ParseMode.Html
                );
                break;
        }
        
    }
}

internal class Discord : MessengerPost
{
    private readonly string _ads = Env.GetString("ADS");
    private readonly string _authorName = Env.GetString("AUTHOR_NAME");
    private readonly string _authorNameRu = Env.GetString("AUTHOR_NAME_RU");
    private readonly string _logoUrl = Env.GetString("LOGO_URL");
    private readonly Models.Post _post = new();

    private readonly DiscordWebhookClient _webhookClient;

    public Discord(DiscordWebhookClient webhookClient, Video video)
    {
        _webhookClient = webhookClient;
        _post = new Models.Post
        {
            PostName = video.Title,
            Description = video.Description,
            Url = $"https://www.youtube.com/watch?v={video.Id}",
            ImageUrl = video.Thumbnail,
            Timestamp = video.PublishedAt ?? DateTime.Now
        };

        // _postParams.status =  PostWorker._videoStatus;

        throw new NotImplementedException();
    }

    public Discord(DiscordWebhookClient webhookClient, Post postFromWordPress)
    {
        _webhookClient = webhookClient;
        _post = new Models.Post
        {
            PostName = postFromWordPress.Title.Rendered,
            Description = Formatter.StripHtml(postFromWordPress.Content.Rendered),
            Url = postFromWordPress.Link,
            ImageUrl = WordPressWorker.Instance.GetMediaUrlById(postFromWordPress.FeaturedMedia).Result,
            Timestamp = postFromWordPress.Date
        };

        var task = new Task(SendPost);
        task.Start();
        task.GetAwaiter().GetResult();

        Console.WriteLine("Discord post from WP sent");
    }

    private async void SendPost()
    {
        var fields = new List<EmbedFieldBuilder>
        {
            new()
            {
                Name = "Полезное",
                Value = _ads,
                IsInline = true
            }
        };

        var author = new EmbedAuthorBuilder()
            .WithName(_authorNameRu)
            .WithIconUrl(_logoUrl);

        var footer = new EmbedFooterBuilder()
            .WithText(_authorName)
            .WithIconUrl(_logoUrl);

        var embed = new EmbedBuilder
        {
            Title = _post.PostName, //$"Семинарус - {_postParams.postName}",
            Author = author,
            Description =  Formatter.Truncate(_post.Description, 4090),
            Color = Color.Orange,
            Fields = fields,
            Footer = footer,
            Url = _post.Url,
            Timestamp = _post.Timestamp,
            ImageUrl = _post.ImageUrl
        };

        // Webhooks are able to send multiple embeds per message
        // As such, your embeds must be passed as a collection.
        await _webhookClient.SendMessageAsync($"@everyone {_post.PostName} - {_post.Url}",
            embeds: new[] { embed.Build() });
    }
}

#endregion