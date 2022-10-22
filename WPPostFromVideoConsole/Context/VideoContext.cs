using Microsoft.EntityFrameworkCore;
using WPPostFromVideoConsole.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace WPPostFromVideoConsole.Context;

public class VideoContext : DbContext
{
#pragma warning disable CS8618
    public VideoContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "video.db");
    }

    public VideoContext(string dbPath)
    {
        DbPath = Path.Join(dbPath, "video.db");
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Post> Posts { get; set; }
    private string DbPath { get; }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
#pragma warning enable CS8618
}