using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Notes
{
    public class UpdateNoteRequestDTO
    {
        [JsonPropertyName("title")]
        public required string? Title { get; set; }

        [JsonPropertyName("content")]
        public required string? Content { get; set; }
    }
}
