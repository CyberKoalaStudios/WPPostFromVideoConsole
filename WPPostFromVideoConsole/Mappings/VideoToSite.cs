using WPPostFromVideoConsole.MediaTypes;

namespace WPPostFromVideoConsole.Mappings;

public static class VideoToSite
{
    public static readonly Dictionary<string, VideoStatus> VideoStatusMap = new()
    {
        { "public", VideoStatus.Public },
        { "private", VideoStatus.Private }
    };

    public static readonly Dictionary<VideoStatus, PostPublishType> VideoStatusToPostMap = new()
    {
        { VideoStatus.Public, PostPublishType.Now },
        { VideoStatus.Private, PostPublishType.Scheduled }
    };
}