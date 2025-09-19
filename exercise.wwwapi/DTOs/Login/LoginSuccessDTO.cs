using exercise.wwwapi.DTOs.Users;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Login;

public class LoginSuccessDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }
        
    [JsonPropertyName("user")]
    public UserDTO User { get; set; }
}