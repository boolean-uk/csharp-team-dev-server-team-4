using exercise.wwwapi.DTOs.Comments;
using FluentValidation;



namespace exercise.wwwapi.Validators.PostValidators
{
    public class CreateCommentsValidator :AbstractValidator<CreateCommentRequestDTO>
    {
        public CreateCommentsValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Comment body cannot be empty.")
                .MaximumLength(1000).WithMessage("Comment body cannot exceed 1000 characters.")
                .MinimumLength(10).WithMessage("Comment body must be at least 10 characters long.");
        }
    }
}
