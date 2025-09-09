using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace exercise.wwwapi.Endpoints
{
    public static class NoteEndpoints
    {
        public static void ConfigureNoteApi(this WebApplication app)
        {
            var notes = app.MapGroup("/users{userId}/notes");
            notes.MapPost("/", CreateNote).WithSummary("Create a note");
            notes.MapGet("/", GetAllNotes).WithSummary("Get all notes");
            //notes.MapGet("/{noteId}", GetNoteById).WithSummary("Get note by id");
            //notes.MapPatch("/{nodeId}", EditNote).WithSummary("Edit note");
            //notes.MapDelete("/{nodeId}", DeleteNote).WithSummary("Edit note");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAllNotes(IRepository<User> userRepository, int userId)
        {
            var user = await userRepository.GetByIdAsync(userId, u => u.Notes);

            if (user is null)
            {
                return TypedResults.NotFound($"User with id {userId} was not found");
            }

            var noteResponse = new List<NoteResponseDTO>();
            foreach (var note in user.Notes)
            {
                noteResponse.Add(new NoteResponseDTO
                {
                    Id = note.Id,
                    UserId = note.UserId,
                    Title = note.Title,
                    Content = note.Content,
                    DateCreated = note.CreatedAt
                });
            }

            var response = new ResponseDTO<NotesResponseDTO>
            {
                Status = "success",
                Data = new NotesResponseDTO
                {
                    Notes = noteResponse
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> CreateNote(IRepository<User> userRepository, NoteRequestDTO request, int userId, IRepository<Note> noteRepository)
        {
            if (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Content))
            {
                return TypedResults.BadRequest("Empty request. Enter title and content");
            }

            var user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return TypedResults.NotFound($"User with id {userId} was not found");
            }

            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            await noteRepository.InsertAsync(note);
            await noteRepository.SaveAsync();

            var noteResponse = new NoteResponseDTO
            {
                Id = note.Id,
                UserId = note.UserId,
                Title = note.Title,
                Content = note.Content,
                DateCreated = note.CreatedAt
            };

            return TypedResults.Ok(noteResponse);
        }

    }
}
