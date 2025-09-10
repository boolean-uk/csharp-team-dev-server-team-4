using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts
{
    public class CreatePostFailureDTO
    {
        [JsonPropertyName("bodyErrors")]
        public List<string> BodyErrors { get; set; } = [];
    }
}
