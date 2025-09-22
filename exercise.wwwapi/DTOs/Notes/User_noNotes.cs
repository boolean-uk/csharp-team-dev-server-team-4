using exercise.wwwapi.DTOs.Users;
using exercise.wwwapi.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.wwwapi.DTOs.Notes
{
    public class GetUser_noNote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User_noNoteDTO User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public GetUser_noNote(){}
        public GetUser_noNote(Note model)
        {
            Id = model.Id;
            UserId = model.UserId;
            User = new User_noNoteDTO(model.User);
            Title = model.Title;
            Content = model.Content;
            CreatedAt = model.CreatedAt;
            UpdatedAt = model.UpdatedAt;
        }
    }
}
