using exercise.wwwapi.DTOs.Notes;
using FluentValidation;

namespace exercise.wwwapi.Validators.NoteValidators;

public class UpdateNoteValidator : AbstractValidator<UpdateNoteRequestDTO>
{
    public UpdateNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("New title must be at least 1 character long")
            .MaximumLength(100).WithMessage("New title cannot exceed 1000 characters");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MinimumLength(1).WithMessage("New content must be at least 1 character long")
            .MaximumLength(100).WithMessage("New content cannot exceed 1000 characters");
    }  
}
