using System.Text.Json.Serialization;
using exercise.wwwapi.Models.UserInfo;

namespace exercise.wwwapi.DTOs.GetUsers;

public class UsersSuccessDTO
{
    [JsonPropertyName("users")] 
    public List<User> Users { get; set; } = [];
}