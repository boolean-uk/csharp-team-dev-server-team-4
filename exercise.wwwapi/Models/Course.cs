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
    [Column("course_name", TypeName = "varchar(100)")]
    public string CourseName { get; set; }

    public ICollection<Module> Modules { get; set; }
    public ICollection<Cohort> Cohorts { get; set; }
}