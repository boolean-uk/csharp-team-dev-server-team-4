using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.GetObjects
{
    public class PostsSuccessDTO
    {
        [JsonPropertyName("posts")]
        public List<Post> Posts { get; set; } = [];
    }
}
