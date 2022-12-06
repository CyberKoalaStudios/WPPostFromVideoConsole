using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using WPPostFromVideoConsole.Models;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace WPPostFromVideoConsole.Context;

public class VideoContext : DbContext
{
#pragma warning disable CS8618
    private string _dbServer;
    private string _dbUser;
    private string _dbPassword;
    private string _dbName;
    public VideoContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "video.db");
 
        _dbServer = Env.GetString("DB_SERVER");
        _dbUser = Env.GetString("DB_USER");
        _dbPassword = Env.GetString("DB_PASSWORD");
        _dbName = Env.GetString("DB_NAME");
    }

    public VideoContext(string dbPath)
    {
        DbPath = Path.Join(dbPath, "video.db");
        
        _dbServer = Env.GetString("DB_SERVER");
        _dbUser = Env.GetString("DB_USER");
        _dbPassword = Env.GetString("DB_PASSWORD");
        _dbName = Env.GetString("DB_TABLE");
    }

    public DbSet<Video> Videos { get; set; }
    public DbSet<Post> Posts { get; set; }
    private string DbPath { get; }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // options.UseMySQL($"server={_dbServer};port=3306;database={_dbName};user={_dbUser};password={_dbPassword}");
        options.UseSqlite($"Data Source={DbPath}");
    }
    
#pragma warning restore CS8618
}