using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("comments")]
public class Comment
{
    [Column("id")]
    public int Id { get; set; }

    [Column("post_id")]
    [ForeignKey(nameof(Post))]
    public int PostId { get; set; }

    [Column("user_id")]
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    [Column("body", TypeName = "varchar(1000)")]
    public string Body { get; set; }

    [Column("create_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Post Post { get; set; }
    public User User { get; set; }
}