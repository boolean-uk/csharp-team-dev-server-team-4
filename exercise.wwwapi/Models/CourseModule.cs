using exercise.wwwapi.Enums;
using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

[Table("Course_Module")]
public class CourseModule : IEntity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey(nameof(Course))]
    [Column("course_id")]
    public int CourseId { get; set; }

    public Course Course { get; set; }

    [ForeignKey(nameof(Exercise))]
    [Column("module_id")]
    public int ModuleId { get; set; }
    public Module Module { get; set; }

}