using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models.UserInfo;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cohort_id")]
    [ForeignKey(nameof(Cohort))]
    public int CohortId { get; set; }

    public Cohort Cohort { get; set; }

    public Credential Credential { get; set; }
    public Profile Profile { get; set; }

    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}