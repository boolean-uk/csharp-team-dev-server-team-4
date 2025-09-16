using exercise.wwwapi.DTOs.Posts.GetPosts;
using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.GetObjects
{
    public class PostsSuccessDTOVol2
    {
        [JsonPropertyName("posts")]
        public List<PostDTOVol2> Posts { get; set; } = [];
    }
}

