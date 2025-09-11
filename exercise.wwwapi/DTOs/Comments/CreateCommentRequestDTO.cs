using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments
{
    public class CreateCommentRequestDTO
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
