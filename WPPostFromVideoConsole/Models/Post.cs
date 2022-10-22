using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WPPostFromVideoConsole.Models;

public class Post
{
#pragma warning disable CS8618
    public Post()
#pragma warning restore CS8618
    {
        PostName = "Post Name";
        Description = "Description";
        Url = "https://cyberkoalastudios.com/category/seminaruses/";
        ImageUrl = "https://cyberkoalastudios.com/wp-content/uploads/2022/10/thumb_bjy3Yua4aSM.jpg";
        Timestamp = DateTimeOffset.Now;
        Status = 0;
    }

    [Key] public int PostId { get; set; }

    public string PostName { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string ImageUrl { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    [Comment("Publish= 0, Future=1, Private=2 .Draft=3, Pending=4,Trash=5")]
    public byte Status { get; set; }

    public int WordpressId { get; set; }
    public int VideoIdx { get; set; }
    public Video Video { get; set; }
}