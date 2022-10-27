using DotNetEnv;
using WordPressPCL.Models;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.CrossPosting;
using WPPostFromVideoConsole.Helpers;
using WPPostFromVideoConsole.Mappings;
using WPPostFromVideoConsole.Models;
using Post = WordPressPCL.Models.Post;

namespace WPPostFromVideoConsole.Workers;

public class PostWorker
{
    private readonly DiscordSender _discordSender = new(Env.GetString("DISCORD_TOKEN"));
    private readonly TelegramSender _telegramSender = new(Env.GetString("TELEGRAM_BOT_TOKEN"));
    private byte _postStatus;

    public PostWorker()
    {
        MyUploads.PostAndVideoPublishedInDb += OnPostWithVideoPublished;

        Publisher.PostStatusChanged += OnPostPublished;
    }

    public void OnPostPublished(Post post)
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
                PutPostInDb(post);
                break;
            }
            case Status.Draft:
                break;
            case Status.Trash:
                break;
            default:
                throw new ArgumentOutOfRangeException(post.Status.ToString());
        }
    }

    public void OnPostWithVideoPublished(Post post, Video video)
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
            case Status.Pending:
            case Status.Private:
            {
                PutPostWithVideoInDb(post, video);
                break;
            }
            case Status.Draft:
                break;
            case Status.Trash:
                break;
            default:
                throw new ArgumentOutOfRangeException(post.Status.ToString());
        }
    }

    // private void OnVideoPublished(Video video)
    // {
    //     var discordPost = _discordSender.CreateFromYouTube(video);
    //     var telegramPost = _telegramSender.CreateFromYouTube(video);
    // }

    private void PutPostInDb(Post post)
    {
        var postParams = new Models.Post
        {
            PostName = post.Title.Rendered,
            Description = Formatter.StripHtml(post.Content.Rendered),
            Url = post.Link,
            ImageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result,
            Timestamp = post.Date,
            WordpressId = post.Id
        };

        PostToDb.PostStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.Status = _postStatus;

        using var db = new VideoContext();
        DbWorker.Instance.PutPostInDb(db, postParams);
    }

    private void PutPostWithVideoInDb(Post post, Video video)
    {
        var postParams = new Models.Post
        {
            PostName = post.Title.Rendered,
            Description = Formatter.StripHtml(post.Content.Rendered),
            Url = post.Link,
            ImageUrl = WordPressWorker.Instance.GetMediaUrlById(post.FeaturedMedia).Result,
            Timestamp = post.Date,
            WordpressId = post.Id
        };

        PostToDb.PostStatusMap.TryGetValue(post.Status, out _postStatus);
        postParams.Status = _postStatus;

        using var db = new VideoContext();
        DbWorker.PutPostWithVideoInDb(db, postParams, video);
    }
}