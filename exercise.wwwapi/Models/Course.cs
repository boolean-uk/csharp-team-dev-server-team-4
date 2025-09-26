using exercise.wwwapi.DTOs.Courses;
using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("courses")]
public class Course : IEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; }

    [Required]
    [Column("specialism_name", TypeName = "varchar(100)")]
    public string SpecialismName { get; set; }

    public ICollection<CohortCourse> CohortCourses { get; set; } = new List<CohortCourse>();
    public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

    public Course(){}

    public Course(CoursePostDTO model)
    {
        Name = model.Name;
        SpecialismName = model.SpecialismName;
    }

}    

