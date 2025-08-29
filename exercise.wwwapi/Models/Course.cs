using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

public class Course
{
    [Column("id", TypeName = "int")]
    public int Id { get; set; }
    
    [Column("name", TypeName = "varchar(100)")]
    public string Name { get; set; }
    
    [Column("cohorts")]
    public ICollection<Cohort> Cohorts { get; set; }
}