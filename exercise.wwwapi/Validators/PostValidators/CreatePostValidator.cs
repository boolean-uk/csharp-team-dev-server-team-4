using exercise.wwwapi.DTOs.Posts;
using FluentValidation;

namespace exercise.wwwapi.Validators.PostValidators
{
    public class CreatePostValidator :AbstractValidator<CreatePostRequestDTO>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Post body cannot be empty.")
                .MaximumLength(1000).WithMessage("Post body cannot exceed 1000 characters.")
                .MinimumLength(1).WithMessage("Post body must be at least 1 characters long.");
        }
    }
}
