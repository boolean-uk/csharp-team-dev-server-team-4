using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class LikeDTO
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }

        public LikeDTO()
        {
            
        }
        public LikeDTO(Like model)
        {
            Id = model.Id;
            PostId = model.PostId;
            UserId = model.UserId;
        }
    }
}
