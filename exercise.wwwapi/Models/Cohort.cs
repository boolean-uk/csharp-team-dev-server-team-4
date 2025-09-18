using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.Models;

[Table("cohorts")]
public class Cohort
{
    [Key] 
    [Column("id")] 
    public int Id { get; set; }

    [Column("cohort_number")]
    public int CohortNumber { get; set; }

    [Column("cohort_name", TypeName = "varchar(50)")]
    public string CohortName { get; set; }

    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    public ICollection<CohortCourse> CohortCourse { get; set; } =  new List<CohortCourse>();
}