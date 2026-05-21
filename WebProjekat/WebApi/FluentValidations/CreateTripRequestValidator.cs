using FluentValidation;
using WebApi.DTOs.Trip;

namespace WebApi.FluentValidations
{
    public class CreateTripRequestValidator : AbstractValidator<CreateTripRequest>
    {
        public CreateTripRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .WithMessage("StartDate cannot be in the past.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate must be after StartDate.");

            RuleFor(x => x.PlannedBudget)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PlannedBudget cannot be negative.");
        }
    }
}
