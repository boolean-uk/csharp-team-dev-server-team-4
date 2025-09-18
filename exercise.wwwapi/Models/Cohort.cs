using exercise.wwwapi.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.Models;

[Table("cohorts")]
public class Cohort : IEntity
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("cohort_number")]
    public int CohortNumber { get; set; }

    [Column("cohort_name", TypeName = "varchar(50)")]
    public string CohortName { get; set; }

    [Column("start_date", TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "date")]
    public DateTime EndDate { get; set; }

    public ICollection<CohortCourse> CohortCourse { get; set; } =  new List<CohortCourse>();
}