using FluentValidation;
using WebApi.DTOs.User;

namespace WebApi.FluentValidations
{
    public class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
    {
        private static readonly string[] ValidRoles = { "User", "Admin" };

        public UpdateUserRoleRequestValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(r => ValidRoles.Contains(r))
                .WithMessage("Role must be either 'User' or 'Admin'.");
        }
    }
}
