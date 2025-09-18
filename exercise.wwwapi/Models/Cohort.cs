using exercise.wwwapi.Models.UserInfo;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("cohorts")]
[Index(nameof(CohortNumber), IsUnique = true)]
public class Cohort
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("cohort_number")]
    public int CohortNumber { get; set; }

    [Column("course_id")]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public ICollection<User> Users { get; set; } =  new List<User>();
}