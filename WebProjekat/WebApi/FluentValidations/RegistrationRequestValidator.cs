using FluentValidation;
using WebApi.DTOs;

namespace WebApi.FluentValidations
{
    public class RegistrationRequestValidator : AbstractValidator<RegistrationRequest>
    {
        public RegistrationRequestValidator() {

            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required.")
                 .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is requeired")
                .Equal("User").WithMessage("Role cannot be different than User");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is requeired")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
