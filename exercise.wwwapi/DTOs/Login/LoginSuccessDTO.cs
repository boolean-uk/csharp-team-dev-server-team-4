namespace exercise.wwwapi.DTOs.Login
{
    public class LoginSuccessDTO
    {
        public string Token { get; set; } = string.Empty;
        public UserDTO User { get; set; } = new();
    }
}