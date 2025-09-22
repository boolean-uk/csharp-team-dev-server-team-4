using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class CommentDTO
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public CommentDTO() { }
        public CommentDTO(Comment model)
        {
            Body = model.Body;
            CreatedAt = model.CreatedAt;
            if (model.User != null)
            {
                firstName = model.User.FirstName;
                lastName = model.User.LastName;
            }
            UpdatedAt = model.UpdatedAt;
            UpdatedBy = model.UpdatedBy;
        }
    }
}
