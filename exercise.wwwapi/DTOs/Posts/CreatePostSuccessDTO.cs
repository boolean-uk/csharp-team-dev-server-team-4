using exercise.wwwapi.DTOs.Posts.GetPosts;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Posts
{
    [NotMapped]
    public class CreatePostSuccessDTO
    {
        public PostDTO Posts { get; set; } = new();
    }
}
