using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Register;

[NotMapped]
public class RegisterRequestDTO
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
        
    [JsonPropertyName("password")]
    public required string Password { get; set; }
        
    [JsonPropertyName("username")]
    public required string Username { get; set; } 
        
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
        
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
        
    [JsonPropertyName("bio")]
    public string? Bio { get; set; }
        
    [JsonPropertyName("github")]
    public string? Github { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("specialism")]
    public Specialism? Specialism { get; set; }
}