using Microsoft.EntityFrameworkCore;
using WPPostFromVideoConsole.Models;

namespace WPPostFromVideoConsole.Context;

public class VideoContext: DbContext
{
    public DbSet<Video> Videos { get; set; }
    public string DbPath { get; }
    
    public VideoContext()
    {
        // var folder = Environment.SpecialFolder.LocalApplicationData;
        // var path = Environment.GetFolderPath(folder);
        string workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

        DbPath = System.IO.Path.Join(projectDirectory, "video.db");
    }
    
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}