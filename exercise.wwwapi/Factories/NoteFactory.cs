using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Factories
{
    public static class NoteFactory
    {
        public static NoteDTO GetNoteDTO(Note note)
        {
            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }
    }
}
