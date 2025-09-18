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

    public string CohortName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}