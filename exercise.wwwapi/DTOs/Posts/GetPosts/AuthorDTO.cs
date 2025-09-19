using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Posts.GetPosts
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public AuthorDTO()
        {
            
        }
        public AuthorDTO(User model)
        {
            Id = model.Id;
            firstName = model.FirstName;
            lastName = model.LastName;
        }
    }
}
