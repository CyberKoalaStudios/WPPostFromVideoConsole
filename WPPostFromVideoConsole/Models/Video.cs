using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WPPostFromVideoConsole.Models;

[Index(nameof(Id), IsUnique = true)]
public class Video
{
    [Key]
    public int Idx { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishedAt { get; set; }

    public string Thumbnail { get; set; }

    [Comment("Wordpress Publication Status; Whether future or now = true. If videopost exist in WP")]
    public bool IsPublished { get; set; }
    // public bool IsSentDiscord { get; set; }
    // public bool IsSentTelegram { get; set; }
    
    public List<Post> PostParams { get; set; }
}