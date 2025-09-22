using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.UpdatePost
{
    public class UpdatePostSuccessDTO
    {
        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("likes")]
        public int Likes { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
