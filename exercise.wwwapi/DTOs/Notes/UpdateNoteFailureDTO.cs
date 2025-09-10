using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Notes
{
    public class UpdateNoteFailureDTO
    {
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }
    }
}
