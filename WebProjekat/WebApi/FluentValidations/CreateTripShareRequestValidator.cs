using FluentValidation;
using TripService.Interfaces.DTOs.TripShare;

namespace WebApi.FluentValidations
{
    public class CreateTripShareRequestValidator : AbstractValidator<CreateTripShareRequest>
    {
        public CreateTripShareRequestValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");

            RuleFor(x => x.AccessType)
                .IsInEnum().WithMessage("Invalid access type.");

            RuleFor(x => x.ExpiresInDays)
                .GreaterThan(0).WithMessage("ExpiresInDays must be greater than zero.")
                .LessThanOrEqualTo(30).WithMessage("Share link cannot exceed 30 days.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
