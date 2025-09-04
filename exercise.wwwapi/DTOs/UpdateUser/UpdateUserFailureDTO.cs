using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.UpdateUser;

public class UpdateUserFailureDTO
{
    [JsonPropertyName("emailErrors")]
    public List<string> EmailErrors { get; set; } = [];
        
    [JsonPropertyName("passwordErrors")]
    public List<string> PasswordErrors { get; set; } = [];
        
    [JsonPropertyName("mobileNumberErrors")]
    public List<string> MobileNumberErrors { get; set; } = [];
}