using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.GetUsers;
using exercise.wwwapi.DTOs.Notes;
using exercise.wwwapi.DTOs.Register;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.UserInfo;
using exercise.wwwapi.Repository;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class NoteEndpoints
    {
        public static void ConfigureNoteApi(this WebApplication app)
        {
            var notes = app.MapGroup("/users/{userId}/notes");
            notes.MapPost("/", CreateNote).RequireAuthorization().WithSummary("Create a note");
            notes.MapGet("/", GetAllNotesForUser).RequireAuthorization().WithSummary("Get all notes for user");
            app.MapGet("notes/{noteId}", GetNoteById).RequireAuthorization().WithSummary("Get note by id");
            app.MapPatch("notes/{noteId}", UpdateNote).RequireAuthorization().WithSummary("Update note");
            app.MapDelete("notes/{noteId}", DeleteNote).RequireAuthorization().WithSummary("Delete note");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetAllNotesForUser(IRepository<User> userRepository, int userId, string? searchTerm, ClaimsPrincipal claimsPrinciple)
        {
            var authorized = AuthorizeTeacher(claimsPrinciple);
            if (!authorized)
            {
                return TypedResults.Unauthorized();
            }

            var user = await userRepository.GetByIdAsync(userId, u => u.Notes);

            if (user is null)
            {
                return TypedResults.NotFound($"User with id {userId} was not found");
            }

            var notes = user.Notes;

            if (!String.IsNullOrWhiteSpace(searchTerm)) 
            {
                notes = notes.Where(
                u => u.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) 
                || u.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                )
                .ToList();
            }

            var noteResponse = new List<NoteResponseDTO>();
            foreach (var note in notes)
            {
                noteResponse.Add(new NoteResponseDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedAt = note.CreatedAt,
                    UpdatedAt = note.UpdatedAt
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> CreateNote(IRepository<User> userRepository, IRepository<Note> noteRepository, 
            CreateNoteRequestDTO request, int userId, ClaimsPrincipal claimsPrinciple, IValidator<CreateNoteRequestDTO> validator)
        {
            var authorized = AuthorizeTeacher(claimsPrinciple);
            if (!authorized)
            {
                return TypedResults.Unauthorized();
            }

            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                var failureDTO = new CreateNoteFailureDTO();

                foreach (var error in validation.Errors)
                {
                    if (error.PropertyName.Equals("title", StringComparison.OrdinalIgnoreCase))
                        failureDTO.TitleErrors.Add(error.ErrorMessage);
                    else if (error.PropertyName.Equals("content", StringComparison.OrdinalIgnoreCase))
                        failureDTO.ContentErrors.Add(error.ErrorMessage);
                }

                var failResponse = new ResponseDTO<CreateNoteFailureDTO> { Status = "fail", Data = failureDTO };
                return Results.BadRequest(failResponse);
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
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };

            noteRepository.Insert(note);
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetNoteById(IRepository<Note> noteRepository, int noteId, ClaimsPrincipal claimsPrinciple)
        {
            var authorized = AuthorizeTeacher(claimsPrinciple);
            if (!authorized)
            {
                return TypedResults.Unauthorized();
            }

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
                    CreatedAt = note.CreatedAt,
                    UpdatedAt = note.UpdatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> DeleteNote(IRepository<Note> noteRepository, int noteId, ClaimsPrincipal claimsPrinciple)
        {
            var authorized = AuthorizeTeacher(claimsPrinciple);
            if (!authorized)
            {
                return TypedResults.Unauthorized();
            }

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
                    CreatedAt = note.CreatedAt,
                    UpdatedAt = note.UpdatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdateNote(IRepository<Note> noteRepository, UpdateNoteRequestDTO request, int noteId, ClaimsPrincipal claimsPrinciple, IValidator<UpdateNoteRequestDTO> validator)
        {
            var authorized = AuthorizeTeacher(claimsPrinciple);
            if (!authorized)
            {
                return TypedResults.Unauthorized();
            }

            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                var failureDTO = new UpdateNoteFailureDTO();

                foreach (var error in validation.Errors)
                {
                    if (error.PropertyName.Equals("title", StringComparison.OrdinalIgnoreCase))
                        failureDTO.TitleErrors.Add(error.ErrorMessage);
                    else if (error.PropertyName.Equals("content", StringComparison.OrdinalIgnoreCase))
                        failureDTO.ContentErrors.Add(error.ErrorMessage);
                }

                var failResponse = new ResponseDTO<UpdateNoteFailureDTO> { Status = "fail", Data = failureDTO };
                return Results.BadRequest(failResponse);
            }

            var note = await noteRepository.GetByIdAsync(noteId);
            if (note is null)
            {
                return TypedResults.NotFound();
            }

            if (request.Title is not null) note.Title = request.Title;
            if (request.Content is not null) note.Content = request.Content;
            note.UpdatedAt = DateTime.UtcNow;

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
                    CreatedAt = note.CreatedAt,
                    UpdatedAt = note.UpdatedAt
                }
            };

            return TypedResults.Ok(response);
        }

        private static bool AuthorizeTeacher(ClaimsPrincipal claims)
        {
            if (claims.IsInRole("Teacher"))
            {
                return true;
            }

            return false;
        }

    }
}
