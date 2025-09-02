using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.DTOs.Login
{
    public class LoginSuccessDTO
    {
        public string Token { get; set; } = string.Empty;
        public User User { get; set; } = new();
    }
}
