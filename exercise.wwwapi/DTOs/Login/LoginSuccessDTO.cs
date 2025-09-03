using exercise.wwwapi.Models;

namespace exercise.wwwapi.DTOs.Login
{
    public class LoginSuccessDTO
    {
        public string token { get; set; }
        public UserDTO user { get; set; } = new UserDTO();
    }
}
