using exercise.wwwapi.Enums;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.UpdateUser;

public class UpdateUserSuccessDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
        
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
        
    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }
        
    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }
        
    [JsonPropertyName("bio")]
    public string? Bio { get; set; }
        
    [JsonPropertyName("github")]
    public string? Github { get; set; }
        
    [JsonPropertyName("Mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("cohortId")]
    public int? CohortId { get; set; }

    [JsonPropertyName("specialism")]
    public Specialism Specialism { get; set; }

    [JsonPropertyName("role")]
    public Role Role { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
}