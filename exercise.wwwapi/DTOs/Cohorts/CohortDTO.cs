
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
    public string StartDateFormatted => $"{StartDate:MMMM} {StartDate:yyyy}";
    public string EndDateFormatted => $"{EndDate:MMMM} {EndDate:yyyy}";
    public List<CourseDTO> Courses { get; set; }

    public CohortDTO(){}
    public CohortDTO(Cohort model)
    {
        Id = model.Id;
        CohortNumber = model.CohortNumber;
        CohortName = model.CohortName;
        StartDate = model.StartDate;
        EndDate = model.EndDate;
        Courses = model.CohortCourses.Select(cc => new CourseDTO(cc.Course)).ToList();
    }
}