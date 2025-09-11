using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Notes
{
    public class CreateNoteFailureDTO
    {
        
        [JsonPropertyName("titleErrors")]
        public List<string> TitleErrors { get; set; } = [];

        [JsonPropertyName("contentErrors")]
        public List<string> ContentErrors { get; set; } = [];
    }
}
