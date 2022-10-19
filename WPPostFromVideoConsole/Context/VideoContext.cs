using Microsoft.EntityFrameworkCore;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Context;

public class VideoContext: DbContext
{
    public DbSet<Video> Videos { get; set; }
    private string DbPath { get; }
    
    public VideoContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        //var workingDirectory = Environment.CurrentDirectory;
        //var projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

        DbPath = Path.Join(path, "video.db");
    }

    public VideoContext(string dbPath)
    {
        DbPath = Path.Join(dbPath, "video.db");
    }
    
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}