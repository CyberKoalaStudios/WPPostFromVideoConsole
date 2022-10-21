using System.Diagnostics.CodeAnalysis;
using Discord;
using Discord.Webhook;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Models;
using WPPostFromVideoConsole.Workers;

namespace WPPostFromVideoConsole.CrossPosting;

internal abstract class MessengerPost { }

#region Messengers

internal class Telegram : MessengerPost
{
    public Telegram()
    {
        Console.WriteLine("Telegram post sended");
    }
}

internal class Discord : MessengerPost
{
    private Video? _video;
    private Post? _post;
    private DiscordWebhookClient _webhookClient;
    private PostParams _postParams;
    private string _logoUrl = DotNetEnv.Env.GetString("LOGO_URL");
    private string _authorName = DotNetEnv.Env.GetString("AUTHOR_NAME");
    private string _authorNameRu = DotNetEnv.Env.GetString("AUTHOR_NAME_RU");
    private string _ads = DotNetEnv.Env.GetString("ADS");
    public Discord(DiscordWebhookClient webhookClient, Video video)
    {
        _webhookClient = webhookClient;
        _video = video;
        
        _postParams.postName = _video.Title;
        _postParams.description = _video.Description;
        _postParams.url = $"https://www.youtube.com/watch?v={_video.Id}";
        _postParams.imageUrl = _video.Thumbnail;
        _postParams.timestamp = _video.PublishedAt ?? DateTime.Now;

        SendPost();
        Console.WriteLine("Discord post from YT sended");
    }
    
    public Discord(DiscordWebhookClient webhookClient, Post post)
    {
        _webhookClient = webhookClient;
        _post = post;
        
        _postParams.postName = _post.Title.Rendered;
        _postParams.description = _post.Content.Rendered;
        _postParams.url = _post.Link;
        _postParams.imageUrl = WordPressWorker.Instance.GetMediaUrlById(_post.FeaturedMedia).Result;
        _postParams.timestamp = _post.Date;
        
        SendPost();
        Console.WriteLine("Discord post from WP sended");
    }

    private async void SendPost()
    {
        var fields = new List<EmbedFieldBuilder>();
        fields.Add(new EmbedFieldBuilder
        {
            Name = "Полезное",
            Value = _ads,
            IsInline = true
        });

        var author = new EmbedAuthorBuilder()
            .WithName(_authorNameRu)
            .WithIconUrl(_logoUrl);

        var footer = new EmbedFooterBuilder()
            .WithText(_authorName)
            .WithIconUrl(_logoUrl);
        
        var embed = new EmbedBuilder
        {
            Title = $"Семинарус - {_postParams.postName}",
            Author = author,
            Description = _postParams.description, // Maybe need trimming
            Color = Color.Orange,
            Fields = fields,
            Footer = footer,
            Url = _postParams.url,
            Timestamp = _postParams.timestamp,
            ImageUrl = _postParams.imageUrl
        };

        // Webhooks are able to send multiple embeds per message
        // As such, your embeds must be passed as a collection.
        await _webhookClient.SendMessageAsync($"@everyone {_postParams.postName} - {_postParams.url}", embeds: new[] { embed.Build() });
    }
    

    struct PostParams
    {
        public string postName;
        public string description;
        public string url;
        public string imageUrl;
        public DateTimeOffset timestamp;
        
        public PostParams()
        {
            postName = "Post Name";
            description = "Description";
            url = "https://cyberkoalastudios.com/category/seminaruses/";
            imageUrl = "https://cyberkoalastudios.com/wp-content/uploads/2022/10/thumb_bjy3Yua4aSM.jpg";
            timestamp = DateTimeOffset.Now;
        }
    }
}

#endregion