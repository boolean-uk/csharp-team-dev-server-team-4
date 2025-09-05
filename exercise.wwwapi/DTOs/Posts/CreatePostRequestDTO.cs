using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts
{
    public class CreatePostRequestDTO
    {
        [JsonPropertyName("author_id")]
        public int AuthorId { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
