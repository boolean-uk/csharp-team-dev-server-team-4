using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
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
    public string Body { get; set; }

    [Column("likes")]
    public int Likes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    //[JsonIgnore]
    public User Author { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}