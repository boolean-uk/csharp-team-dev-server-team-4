using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.UpdatePost
{
    public class UpdatePostFailureDTO
    {
        [JsonPropertyName("bodyErrors")]
        public List<string> BodyErrors { get; set; } = [];
    }
}
