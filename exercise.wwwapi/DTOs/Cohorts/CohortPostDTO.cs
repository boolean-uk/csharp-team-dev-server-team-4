
using System.Text.Json.Serialization;

public class CohortPostDTO
{
    [JsonPropertyName("course_id")]
    public int CourseId { get; set; }
}