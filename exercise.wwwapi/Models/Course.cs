using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("course")]
public class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("course_name")]
    public string CourseName { get; set; } = null!;

    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<Cohort> Cohorts { get; set; } = new List<Cohort>();
}