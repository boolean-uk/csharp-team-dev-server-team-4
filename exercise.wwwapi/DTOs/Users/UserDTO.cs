using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Factories;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Users;

public class UserDTO
{       
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

    public int? CohortId { get; set; }
    public int? CohortCourseId { get; set; }

    public DateTime? CurrentStartdate { get; set; }
    public DateTime? CurrentEnddate { get; set; }


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
        CohortId = model.User_CC?.LastOrDefault()?.CohortCourse.CohortId;
        CohortCourseId = model.User_CC?.LastOrDefault()?.Id; //autofetching the first element of usercc
        CurrentStartdate = model.User_CC?.LastOrDefault()?.CohortCourse.Cohort.StartDate; //autofetching the first element of usercc
        CurrentEnddate = model.User_CC?.LastOrDefault()?.CohortCourse.Cohort.EndDate; //autofetching the first element of usercc
        Notes = model.Notes.Select(n => new NoteDTO(n)).ToList();
    }

    public UserDTO(User model, PrivilegeLevel privilegeLevel)
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
        CohortId = model.User_CC?.LastOrDefault()?.CohortCourse.CohortId;
        CohortCourseId = model.User_CC?.LastOrDefault()?.CohortCourse.CohortId; //autofetching the first element of usercc
        CurrentStartdate = model.User_CC?.LastOrDefault()?.CohortCourse.Cohort.StartDate; //autofetching the first element of usercc
        CurrentEnddate = model.User_CC?.LastOrDefault()?.CohortCourse.Cohort.EndDate; //autofetching the first element of usercc
        

        if (privilegeLevel == PrivilegeLevel.Teacher)
        {
            Notes = model.Notes.Select(n => new NoteDTO(n)).ToList();
        }
    }
}