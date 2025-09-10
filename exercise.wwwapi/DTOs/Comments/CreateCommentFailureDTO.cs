using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments
{
    public class CreateCommentFailureDTO
    {
        [JsonPropertyName("bodyErrors")]
        public List<string> BodyErrors { get; set; } = new();
    }
}
