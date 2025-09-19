using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.GetObjects
{
    public class PostsSuccessDTO
    {
        [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; } = [];
    }
}
