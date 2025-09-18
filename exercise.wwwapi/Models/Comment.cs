using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

[Table("comments")]
public class Comment : IEntity
{
    [Key]
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

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public Post Post { get; set; }
    
    [JsonIgnore]
    public User User { get; set; }
}