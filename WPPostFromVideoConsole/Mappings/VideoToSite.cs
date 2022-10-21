using WPPostFromVideoConsole.MediaTypes;

namespace WPPostFromVideoConsole.Mappings;

public class VideoToSite
{
    public static readonly Dictionary<string, VideoStatus> videoStatusMap = new()
    {
        { "public", VideoStatus.Public },
        { "private", VideoStatus.Private},
    };
    
    public static readonly Dictionary<VideoStatus, PostPublishType> videoStatusToPostMap = new()
    {
        { VideoStatus.Public, PostPublishType.Now },
        { VideoStatus.Private, PostPublishType.Scheduled},
    };
}