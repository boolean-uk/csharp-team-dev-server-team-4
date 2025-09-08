using exercise.wwwapi.DTOs.Comments.UpdateComment;
using exercise.wwwapi.DTOs.Posts.UpdatePost;
using FluentValidation;

namespace exercise.wwwapi.Validators.PostValidators
{
    public class UpdateCommentsValidator : AbstractValidator<UpdateCommentRequestDTO>
    {
        public UpdateCommentsValidator()
        {
            RuleFor(x => x.Body)
                    .NotEmpty().When(x => x.Body != null)
                    .WithMessage("Comment body cannot be empty if provided.")
                    .MaximumLength(1000).WithMessage("Comment body cannot exceed 1000 characters.")
                    .MinimumLength(10).When(x => !string.IsNullOrWhiteSpace(x.Body))
                    .WithMessage("Comment body must be at least 10 characters long.");
        }
    }
}
