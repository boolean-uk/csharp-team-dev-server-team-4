using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class ProfileDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }

        public ProfileDTO()
        {
            
        }
        public ProfileDTO(Profile model)
        {
            firstName = model.FirstName;
            lastName = model.LastName;
        }
    }
}
