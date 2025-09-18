using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class PostDTOVol2
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public List<LikeDTO> Likes { get; set; } = new List<LikeDTO>();

        public PostDTOVol2()
        {
            
        }
        public PostDTOVol2(Post model)
        {
            Id = model.Id;
            AuthorId = model.AuthorId;
            Body = model.Body;
            CreatedAt = model.CreatedAt;
            Firstname = model.Author.FirstName;
            Lastname = model.Author.LastName;
            Comments = model.Comments.Select(c => new CommentDTO(c)).ToList();
            Likes = model.Likes.Select(l => new LikeDTO(l)).ToList();
        }
    }
}
