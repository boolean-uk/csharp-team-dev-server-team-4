using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs
{
    public class ResponseDTO<T> where T : new()
    {
        [JsonPropertyName("status")] 
        public string Status { get; set; } = string.Empty;
        
        [JsonPropertyName("data")] 
        public T Data { get; set; } = new T();
    }
}