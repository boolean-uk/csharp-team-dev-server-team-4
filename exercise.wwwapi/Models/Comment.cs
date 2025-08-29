using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class Comment
{
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [Column("content", TypeName = "varchar(500)")]
    public string Content { get; set; }
    
    [ForeignKey("Post")]
    [Column("post_id", TypeName = "int")]
    public int PostId { get; set; }
    
    [Column("post")]
    public Post Post { get; set; }
    
    [ForeignKey("User")]
    [Column("user_id", TypeName = "int")]
    public int UserId { get; set; }
    
    [Column("user")]
    public User User { get; set; }
}