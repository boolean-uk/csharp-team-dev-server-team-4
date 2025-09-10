using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments.UpdateComment
{
    public class UpdateCommentRequestDTO
    {
        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
