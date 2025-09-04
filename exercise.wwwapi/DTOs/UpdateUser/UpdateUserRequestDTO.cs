using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.UpdateUser;

public class UpdateUserRequestDTO
{
    [JsonPropertyName("email")]
    public string? Email { get; set; }
        
    [JsonPropertyName("password")]
    public string? Password { get; set; }
        
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
        
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
        
    [JsonPropertyName("bio")]
    public string? Bio { get; set; }
        
    [JsonPropertyName("github")]
    public string? Github { get; set; }
        
    [JsonPropertyName("username")]
    public string? Username { get; set; }
        
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }
}