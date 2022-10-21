using WordPressPCL.Models;
using WPPostFromVideoConsole.MediaTypes;

namespace WPPostFromVideoConsole.Mappings;

public class PostToDb
{
    public static readonly Dictionary<Status, byte> postStatusMap = new()
    {
        { Status.Publish, 0 },
        { Status.Future, 1 },
        { Status.Private, 2 },
        { Status.Draft, 3},
        { Status.Pending, 4},
        { Status.Trash, 5}
    };
    
    public static readonly Dictionary<int, PostPublishType> postTypeDict = new()
    {
        { 0, PostPublishType.Scheduled },
        { 1, PostPublishType.Now },
        { 2, PostPublishType.AtDate }
    };

}
