using exercise.wwwapi.Enums;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Users
{
    public class PatchUserDTO
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Github { get; set; }
        public string? Username { get; set; }
        public string? Mobile { get; set; }
        public Role? Role { get; set; }
    }
}
