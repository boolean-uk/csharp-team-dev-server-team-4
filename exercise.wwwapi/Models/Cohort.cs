using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class Cohort
{
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [Column("number", TypeName = "int")]
    public int Number { get; set; }
    
    [ForeignKey("Course")]
    [Column("course_id", TypeName = "int")]
    public int CourseId { get; set; }

    [Column("course")]
    public Course Course { get; set; }
    
    [Column("users")]
    public ICollection<User> Users { get; set; }
}