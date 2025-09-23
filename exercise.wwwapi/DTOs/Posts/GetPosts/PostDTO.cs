using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class PostDTO
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();

        public PostDTO() { }
        public PostDTO(Post model)
        {
            Id = model.Id;
            AuthorId = model.AuthorId;
            Firstname = model.Author.FirstName;
            Lastname = model.Author.LastName;
            Body = model.Body;
            CreatedAt = model.CreatedAt;
            Comments = model.Comments.Select(c => new CommentDTO(c)).ToList();
            Likes = model.Likes.Select(l => new LikeDTO(l)).ToList();
            UpdatedAt = model.UpdatedAt;
            UpdatedBy = model.UpdatedBy;
        }
    }
}
