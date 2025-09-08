using exercise.wwwapi.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs;

public class UserDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("email")]
    public string Email { get; set; }
        
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

    [JsonPropertyName("startDate")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("specialism")]
    public Specialism? Specialism { get; set; }

    [JsonPropertyName("cohortId")]
    public int? CohortId { get; set; }
}