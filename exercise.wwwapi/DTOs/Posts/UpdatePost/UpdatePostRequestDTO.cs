using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.UpdatePost
{
    public class UpdatePostRequestDTO
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
