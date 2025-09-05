using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models.UserInfo;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    public Credential Credential { get; set; }
    public Profile Profile { get; set; }

    public ICollection<Post> Posts { get; set; }
    public ICollection<Comment> Comments { get; set; }
    [ForeignKey(nameof(Cohort))]
    public int CohortId { get; set; }
    [JsonIgnore]
    public Cohort? Cohort { get; set; }
}