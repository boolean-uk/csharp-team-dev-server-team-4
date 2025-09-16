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
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();

        public PostDTOVol2()
        {
            
        }
        public PostDTOVol2(Post model)
        {
            Id = model.Id;
            AuthorId = model.AuthorId;
            Body = model.Body;
            Likes = model.Likes;
            CreatedAt = model.CreatedAt;
            Firstname = model.Author.Profile.FirstName;
            Lastname = model.Author.Profile.LastName;
            Comments = model.Comments.Select(c => new CommentDTO(c)).ToList();
        }
    }
}
