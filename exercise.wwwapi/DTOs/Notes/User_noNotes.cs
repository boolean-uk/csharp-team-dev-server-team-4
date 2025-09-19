using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Notes
{
    public class User_noNotes
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public Role? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Github { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }
        public Specialism? Specialism { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserExercise> User_Exercises { get; set; } = new List<UserExercise>();
        public ICollection<UserCC> User_CC { get; set; } = new List<UserCC>();

        public User_noNotes(){}
        public User_noNotes(User model)
        {
            Id = model.Id;
            Username = model.Username;
            Email = model.Email;
            PasswordHash = model.PasswordHash;
            Role = model.Role;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Mobile = model.Mobile;
            Github = model.Github;
            Bio = model.Bio;
            PhotoUrl = model.PhotoUrl;
            Specialism = model.Specialism;
        }
    }
}
