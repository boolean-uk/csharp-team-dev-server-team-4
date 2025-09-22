using exercise.wwwapi.Enums;
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
        
    public string? Bio { get; set; }
        
    [JsonPropertyName("github")]
    public string? Github { get; set; }
        
    [JsonPropertyName("username")]
    public string? Username { get; set; }
        
    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("cohortId")]
    public int? CohortId { get; set; }

    [JsonPropertyName("specialism")]
    public Specialism? Specialism { get; set; }

    [JsonPropertyName("role")]
    public Role? Role { get; set; }

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }
}