using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Login;

public class LoginFailureDTO
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "Invalid email and/or password provided";
}