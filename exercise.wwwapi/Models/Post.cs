using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class Post
{
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [Column("content", TypeName = "varchar(500)")]
    public string Content { get; set; }

    [Column("created_at", TypeName = "timestamp with time zone")]
    public DateTime CreateMoment { get; init; } = DateTime.UtcNow;
    
    [Column("comments")]
    public ICollection<Comment> Comments { get; set; }
    
    [Column("like_count", TypeName = "int")]
    public int LikeCount { get; set; }
    
    [ForeignKey("User")]
    [Column("user_id", TypeName = "int")]
    public int UserId { get; set; }
    
    [Column("user")]
    public User User { get; set; }
}