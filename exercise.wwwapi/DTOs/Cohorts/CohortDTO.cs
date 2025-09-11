using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Cohorts
{
    public class CohortDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("course_id")]
        public int CourseId { get; set; }
    }
}
