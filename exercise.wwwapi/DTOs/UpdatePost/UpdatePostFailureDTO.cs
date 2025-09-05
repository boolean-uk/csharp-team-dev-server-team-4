using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.UpdatePost
{
    public class UpdatePostFailureDTO
    {
        [JsonPropertyName("bodyErrors")]
        public List<string> BodyErrors { get; set; } = [];
    }
}
