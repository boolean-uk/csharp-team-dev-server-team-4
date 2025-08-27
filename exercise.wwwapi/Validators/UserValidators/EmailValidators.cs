using exercise.wwwapi.DTOs.Register;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace exercise.wwwapi.Validators.UserValidators
{
    public class EmailValidators : AbstractValidator<RegisterRequestDTO>
    {
        public EmailValidators()
        {
            RuleFor(x => x.email)
                .EmailAddress().WithMessage("The email address must be in a valid format");
        }
    }
}
