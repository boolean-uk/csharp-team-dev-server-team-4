using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        //public ProfileDTO Profile { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public AuthorDTO()
        {
            
        }
        public AuthorDTO(User model)
        {
            Id = model.Id;
            //Profile = new ProfileDTO(model.Profile);
            firstName = model.Profile.FirstName;
            lastName = model.Profile.LastName;
        }
    }
}
