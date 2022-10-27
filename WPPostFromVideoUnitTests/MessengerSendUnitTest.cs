using WordPressPCL.Models;
using WPPostFromVideoConsole;
using WPPostFromVideoConsole.Models;
using WPPostFromVideoConsole.Workers;
using Post = WordPressPCL.Models.Post;

namespace WPPostFromVideoUnitTests;

public class MessengerSendUnitTest
{
    private PostWorker _postWorker;
    private Video _mockVideo;
  
    [SetUp]
    public void Setup()
    {
        DotNetEnv.Env.Load();
        _postWorker = new PostWorker();
        _mockVideo = new Video
        {
            Id = "AbCdEf",
            Description = "Test Description",
            PublishedAt = DateTime.Now,
            Title = "Video Title",
            Thumbnail = "https://www.wyzowl.com/wp-content/uploads/2019/09/YouTube-thumbnail-size-guide-best-practices-top-examples.png",
            IsPublished = false
        };
    }

    [Test]
    public void FuturePostWithVideoTest()
    {
        var testPost =  WordPressWorker.Instance.GetPostById(9094).Result;
        testPost.Status = Status.Future;

        _postWorker.OnPostWithVideoPublished(testPost, _mockVideo);
    }

    [Test]
    public void PublishedPostWithVideoTest()
    {
        var testPost =  WordPressWorker.Instance.GetPostById(9094).Result;
        testPost.Status = Status.Publish;
        
        _postWorker.OnPostWithVideoPublished(testPost, _mockVideo);
    }
}