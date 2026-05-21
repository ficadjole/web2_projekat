using FluentValidation;
using WebApi.DTOs.Trip;

namespace WebApi.FluentValidations
{
    public class UpdateTripRequestValidator : AbstractValidator<UpdateTripRequest>
    {
        public UpdateTripRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate must be after StartDate.");

            RuleFor(x => x.PlannedBudget)
                .GreaterThanOrEqualTo(0)
                .WithMessage("PlannedBudget cannot be negative.");
        }
    }
}
