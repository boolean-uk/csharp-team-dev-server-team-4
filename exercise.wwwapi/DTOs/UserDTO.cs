using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs
{
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
    }
}