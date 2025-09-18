
using System.Text.Json.Serialization;

public class CohortPostDTO
{
    [JsonPropertyName("course_id")]
    public int CourseId { get; set; }

    public string CohortName { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}