using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Register
{
    [NotMapped]
    public class RegisterRequestDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Username { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? GithubUrl { get; set; }
    }
}
