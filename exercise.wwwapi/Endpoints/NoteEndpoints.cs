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
            notes.MapGet("/", GetAllNotesForUser).WithSummary("Get all notes for user");
            app.MapGet("notes/{noteId}", GetNoteById).WithSummary("Get note by id");
            app.MapPatch("/{nodeId}", EditNote).WithSummary("Edit note");
            app.MapDelete("/{nodeId}", DeleteNote).WithSummary("Delete note");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAllNotesForUser(IRepository<User> userRepository, int userId)
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
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
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

            var response = new ResponseDTO<NoteResponseDTO>
            {
                Status = "success",
                Data = new NoteResponseDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetNoteById(IRepository<Note> noteRepository, int noteId)
        {
            var note = await noteRepository.GetByIdAsync(noteId);
            if (note is null)
            {
                return TypedResults.NotFound();
            }

            var response = new ResponseDTO<NoteResponseDTO>
            {
                Status = "success",
                Data = new NoteResponseDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> DeleteNote(IRepository<Note> noteRepository, int noteId)
        {
            var note = await noteRepository.GetByIdAsync(noteId);
            if (note is null)
            {
                return TypedResults.NotFound();
            }

            noteRepository.Delete(note);
            await noteRepository.SaveAsync();

            var response = new ResponseDTO<NoteResponseDTO>
            {
                Status = "success",
                Data = new NoteResponseDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> EditNote(IRepository<Note> noteRepository, UpdateNoteRequestDTO request, int noteId)
        {
            if (string.IsNullOrWhiteSpace(request.Title) && string.IsNullOrWhiteSpace(request.Content))
            {
                return TypedResults.BadRequest("Empty request. Enter title and content");
            }

            var note = await noteRepository.GetByIdAsync(noteId);
            if (note is null)
            {
                return TypedResults.NotFound();
            }

            if (request.Title is not null) note.Title = request.Title;
            if (request.Content is not null) note.Content = request.Content;

            noteRepository.Update(note);
            await noteRepository.SaveAsync();

            var response = new ResponseDTO<NoteResponseDTO>
            {
                Status = "success",
                Data = new NoteResponseDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt
                }
            };

            return TypedResults.Ok(response);
        }

    }
}
