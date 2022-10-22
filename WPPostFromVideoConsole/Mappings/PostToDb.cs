using WordPressPCL.Models;

namespace WPPostFromVideoConsole.Mappings;

public static class PostToDb
{
    public static readonly Dictionary<Status, byte> PostStatusMap = new()
    {
        { Status.Publish, 0 },
        { Status.Future, 1 },
        { Status.Private, 2 },
        { Status.Draft, 3 },
        { Status.Pending, 4 },
        { Status.Trash, 5 }
    };
}