using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Users;

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
        
    [JsonPropertyName("mobile")]
    public string? Mobile { get; set; }

    [JsonPropertyName("specialism")]
    public Specialism? Specialism { get; set; }

    [JsonPropertyName("notes")]
    
    public ICollection<NoteDTO> Notes { get; set; } = new List<NoteDTO>();
    [JsonPropertyName("role")]
    public string Role { get; set; }

    public UserDTO()
    {
        
    }

    public UserDTO(User model)
    {
        Id = model.Id;
        Email = model.Email;
        FirstName = model.FirstName;
        LastName = model.LastName;
        Bio = model.Bio;
        Github = model.Github;
        Username = model.Username;
        Mobile = model.Mobile;
        Specialism = model.Specialism;
        Role = model.Role.ToString();
        Notes = model.Notes.Select(n => new NoteDTO(n)).ToList();
    }
}