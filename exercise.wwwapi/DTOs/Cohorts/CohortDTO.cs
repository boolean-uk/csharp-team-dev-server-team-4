using exercise.wwwapi.Models.UserInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.Models;

public class CohortDTO
{
    [JsonPropertyName("course_id")]
    public int CourseId { get; set; }
    [JsonPropertyName("cohort_number")]
    public int CohortNumber { get; set; }
    public CourseDTO? Course { get; set; }
}