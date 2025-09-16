using exercise.wwwapi.Models;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class PostDTOVol2
    {
        [JsonPropertyName("author")]
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; } = string.Empty;
        public int Likes { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
        public string firstname { get; set; }
        public string lastname { get; set; }

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
            firstname = model.Author.Profile.FirstName;
            lastname = model.Author.Profile.LastName;
            Comments = model.Comments.Select(c => new CommentDTO(c)).ToList();
        }
    }
}
