using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Login;

[NotMapped]
public class LoginRequestDTO
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
        
    [JsonPropertyName("password")]
    public string? Password { get; set; }        
}