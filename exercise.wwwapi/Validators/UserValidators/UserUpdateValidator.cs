using exercise.wwwapi.DTOs.UpdateUser;
using FluentValidation;

namespace exercise.wwwapi.Validators.UserValidators;

public class UserUpdateValidator : AbstractValidator<UpdateUserRequestDTO>
{
    public UserUpdateValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("The email address must be in a valid format");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password too short");

        RuleFor(x => x.Password)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.");

        RuleFor(x => x.Password)
            .Matches(@"\d+").WithMessage("Password must contain at least one number");

        RuleFor(x => x.Password)
            .Matches(@"[^a-zA-Z0-9\s]+").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.Phone)
            .Matches(@"^+?\d{7,15}$").WithMessage("Phone must be a valid phone number.");
    }
}