using exercise.wwwapi.DTOs.Posts.GetPosts;
using System.Text.Json.Serialization;


public class CommentsSuccessDTO
{
    [JsonPropertyName("comments")]
    public List<CommentDTO> Comments { get; set; } = new();
}
