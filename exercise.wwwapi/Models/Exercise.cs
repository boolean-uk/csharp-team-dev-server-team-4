using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("exercise")]
public class Exercise
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unit_id")]
    [ForeignKey(nameof(Unit))]
    public int UnitId { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; }

    [Required]
    [Column("github_link", TypeName = "varchar(200)")]
    public string GitHubLink { get; set; }

    [Required]
    [Column("description", TypeName = "varchar(500)")] 
    public string Description { get; set; }

    public Unit Unit { get; set; }
}