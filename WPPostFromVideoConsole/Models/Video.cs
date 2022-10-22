using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WPPostFromVideoConsole.Models;

#pragma warning disable CS8618
// ReSharper disable once UnusedAutoPropertyAccessor.Global
// ReSharper disable once CollectionNeverUpdated.Global
[Index(nameof(Id), IsUnique = true)]
public class Video
{
    [Key] public int Idx { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string Thumbnail { get; set; }

    [Comment("Wordpress Publication Status; Whether future or now = true. If video post exist in WP")]
    public bool IsPublished { get; set; }

    public List<Post> PostParams { get; set; }
}
#pragma warning restore CS8618