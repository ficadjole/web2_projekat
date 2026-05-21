using FluentValidation;
using WebApi.DTOs.Activity;

namespace WebApi.FluentValidations
{
    public class CreateActivityRequestValidator : AbstractValidator<CreateActivityRequest>
    {
        public CreateActivityRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(x => x.EstimatedCost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("EstimatedCost cannot be negative.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid activity status.");

            RuleFor(x => x.DestinationId)
                .NotEmpty().WithMessage("DestinationId is required.");
        }
    }
}
