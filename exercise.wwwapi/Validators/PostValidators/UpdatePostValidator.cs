using exercise.wwwapi.DTOs.Posts.UpdatePost;
using FluentValidation;

namespace exercise.wwwapi.Validators.PostValidators
{
    public class UpdatePostValidator : AbstractValidator<UpdatePostRequestDTO>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.Body)
                    .NotEmpty().When(x => x.Body != null)
                    .WithMessage("Post body cannot be empty if provided.")
                    .MaximumLength(1000).WithMessage("Post body cannot exceed 1000 characters.")
                    .MinimumLength(10).When(x => !string.IsNullOrWhiteSpace(x.Body))
                    .WithMessage("Post body must be at least 10 characters long.");
        }
    }
}
