using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments.UpdateComment
{
    public class UpdateCommentFailureDTO
    {
        [JsonPropertyName("bodyErrors")]
        public List<string> BodyErrors { get; set; } = new();
    }
}
