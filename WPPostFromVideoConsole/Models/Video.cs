using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WPPostFromVideoConsole.Models;

public class Video
{
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Key, Column(Order = 0)]
    // public int Index { get; set; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishedAt { get; set; }
    
    public string Thumbnail { get; set; }
    
    public bool IsPublished { get; set; }
}