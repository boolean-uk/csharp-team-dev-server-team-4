using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Comments
{
    public class CommentDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("post_id")]
        public int PostId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
