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

    public bool IsPublished { get; set; }
    
    public List<PostParams> PostParams { get; set; }
}