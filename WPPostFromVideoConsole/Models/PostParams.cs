using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WPPostFromVideoConsole.Models;

public class PostParams
{
    [Key]
    public int PostId { get; set; }
    public string postName { get; set; }
    public string description { get; set; }
    public string url { get; set; }
    public string imageUrl { get; set; }
    public DateTimeOffset timestamp { get; set; }
    public byte status { get; set; }
    
    public int VideoIdx { get; set; }
    public Video Video { get; set; }
        
    public PostParams()
    {
        postName = "Post Name";
        description = "Description";
        url = "https://cyberkoalastudios.com/category/seminaruses/";
        imageUrl = "https://cyberkoalastudios.com/wp-content/uploads/2022/10/thumb_bjy3Yua4aSM.jpg";
        timestamp = DateTimeOffset.Now;
        status = 0;
    }
}