using exercise.wwwapi.DTOs.Notes;
using FluentValidation;

namespace exercise.wwwapi.Validators.NoteValidators;

public class CreateNoteValidator : AbstractValidator<CreateNoteRequestDTO>
{
    public CreateNoteValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MinimumLength(1).WithMessage("Title must be at least 1 character long")
            .MaximumLength(100).WithMessage("Title cannot exceed 1000 characters");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required")
            .MinimumLength(1).WithMessage("Content must be at least 1 character long")
            .MaximumLength(100).WithMessage("Content cannot exceed 1000 characters");
    }
}
