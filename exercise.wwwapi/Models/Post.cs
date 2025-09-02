using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.Models;

[Table("posts")]
public class Post
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("author_id")]
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }

    [Required]
    [Column("body", TypeName = "varchar(1000)")]
    public string Body { get; set; } = string.Empty;

    [Column("likes")]
    public int Likes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Author { get; set; } = null!;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}