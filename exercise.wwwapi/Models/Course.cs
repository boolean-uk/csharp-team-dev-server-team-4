using exercise.wwwapi.Models.UserInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("courses")]
public class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; }

    public ICollection<CohortCourse> CohortCourses { get; set; } = new List<CohortCourse>();
    public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

}    

