using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("modules")]
public class Module
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; }
        
    public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();
    public ICollection<Unit> Units { get; set; } = new List<Unit>();
}
