using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Register;

public class RegisterFailureDTO
{
    [JsonPropertyName("emailErrors")]
    public List<string> EmailErrors { get; set; } = [];
        
    [JsonPropertyName("passwordErrors")]
    public List<string> PasswordErrors { get; set; } = [];
}