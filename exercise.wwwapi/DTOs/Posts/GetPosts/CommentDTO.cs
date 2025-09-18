using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public CommentDTO()
        {
        }
        public CommentDTO(Comment model)
        {
            Id = model.Id;
            UserId = model.UserId;
            Body = model.Body;
            CreatedAt = model.CreatedAt;
            firstName = model.User.FirstName;
            lastName = model.User.LastName;
        }
    }
}
