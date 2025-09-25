using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Notes
{
    public class User_noNoteDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Role? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Mobile { get; set; }
        public string? Github { get; set; }
        public string? Bio { get; set; }
        public string? PhotoUrl { get; set; }

        public User_noNoteDTO() { }
        public User_noNoteDTO(User model)
        {
            Id = model.Id;
            Username = model.Username;
            Email = model.Email;
            Role = model.Role;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Mobile = model.Mobile;
            Github = model.Github;
            Bio = model.Bio;
            PhotoUrl = model.PhotoUrl;
        }
    }
}