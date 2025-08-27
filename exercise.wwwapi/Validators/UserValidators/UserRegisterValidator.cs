using exercise.wwwapi.DTOs.Register;
using FluentValidation;

namespace exercise.wwwapi.Validators.UserValidators
{
    public class UserRegisterValidator : AbstractValidator<RegisterRequestDTO>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.email)
                .EmailAddress().WithMessage("The email address must be in a valid format");

            RuleFor(x => x.password)
                .MinimumLength(8)
                .WithMessage("Password too short");

            RuleFor(x => x.password)
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.");

            RuleFor(x => x.password)
                .Matches(@"\d+").WithMessage("Password must contain at least one number");

            RuleFor(x => x.password)
                .Matches(@"[^a-zA-Z0-9\s]+").WithMessage("Password must contain at least one special character");
        }
    }
}
