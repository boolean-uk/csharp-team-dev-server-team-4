
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using exercise.wwwapi.DTOs.Courses;
using exercise.wwwapi.DTOs.Exercises;

namespace exercise.wwwapi.Models;

public class CohortDTO
{
    public int Id { get; set; }
    public int CohortNumber { get; set; }
    public string CohortName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<CourseDTO> Courses { get; set; }
}