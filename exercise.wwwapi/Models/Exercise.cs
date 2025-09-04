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
    [Column("title", TypeName = "varchar(100)")]
    public string Title { get; set; }

    [Column("description", TypeName = "varchar(500)")] 
    public string? Description { get; set; }

    public Unit Unit { get; set; }
}