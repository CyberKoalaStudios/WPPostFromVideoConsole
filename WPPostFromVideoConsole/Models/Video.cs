namespace WPPostFromVideoConsole.Models;

public class Video
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? PublishedAt { get; set; }

    public string Thumbnail { get; set; }

    public bool IsPublished { get; set; }
}