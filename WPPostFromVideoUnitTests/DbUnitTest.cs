using Microsoft.EntityFrameworkCore;
using WPPostFromVideoConsole.Context;
using WPPostFromVideoConsole.Models;
using WPPostFromVideoConsole.Workers;

namespace WPPostFromVideoUnitTests;

public class Tests
{
    private VideoContext _context;
    private Video? _video;

    [SetUp]
    public void Setup()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _context = new VideoContext(path);
        _context.Database.Migrate();
    }

    [Test]
    public void IsDbNull()
    {
        _video = DbWorker.Instance.GetVideoFromDb(_context);
        Assert.IsNotNull(_video.Id);
    }

    // [Test]
    // public void InsertNonUniqueDbTest()
    // {
    //     _video = DbWorker.Instance.GetVideoFromDb(_context);
    //     var addedToDb = DbWorker.Instance.AddVideoToDb(_context, _video);
    //     Assert.GreaterOrEqual(addedToDb, 0);
    // }
}