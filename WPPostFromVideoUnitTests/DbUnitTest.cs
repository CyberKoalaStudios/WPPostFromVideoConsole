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
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName +
                               "/WPPostFromVideoConsole";
        _context = new VideoContext(projectDirectory);
        _context.Database.Migrate();
    }

    [Test]
    public void IsDbNull()
    {
        _video = DbWorker.Instance.GetVideoFromDb(_context);
        Assert.IsNotNull(_video.Id);
    }

    [Test]
    public void InsertNonUniqueDbTest()
    {
        _video = DbWorker.Instance.GetVideoFromDb(_context);
        var addedToDb = DbWorker.Instance.AddVideoToDb(_video, _context);
        Assert.GreaterOrEqual(addedToDb, 0);
    }
}