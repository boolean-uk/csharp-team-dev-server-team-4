using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("units")]
public class Unit
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("module_id")]
    [ForeignKey(nameof(Module))]
    public int ModuleId { get; set; }
    public Module Module { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }
        
    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}