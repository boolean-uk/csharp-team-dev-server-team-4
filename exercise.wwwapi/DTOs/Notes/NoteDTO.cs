using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace exercise.wwwapi.DTOs.Notes
{
    public class NoteDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updatedat")]
        public DateTime UpdatedAt { get; set; }
        public NoteDTO()
        {
            
        }
        public NoteDTO(Note model)
        {
            Id = model.Id;
            Title = model.Title;
            Content = model.Content;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;

        }
    }
}
