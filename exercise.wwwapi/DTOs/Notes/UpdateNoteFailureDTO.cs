using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Notes
{
    public class UpdateNoteFailureDTO
    {
        [JsonPropertyName("titleErrors")]
        public List<string> TitleErrors { get; set; } = [];

        [JsonPropertyName("contentErrors")]
        public List<string> ContentErrors { get; set; } = [];
    }
}
