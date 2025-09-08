using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments
{
    public class CommentsSuccessDTO
    {
        [JsonPropertyName("comments")]
        public List<CommentDTO> Comments { get; set; } = new();
    }
}
