using FluentValidation;
using WebApi.DTOs.Destination;

namespace WebApi.FluentValidations
{
    public class CreateDestinationRequestValidator : AbstractValidator<CreateDestinationRequest>
    {
        public CreateDestinationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

            RuleFor(x => x.ArrivingDate)
                .NotEmpty().WithMessage("ArrivingDate is required.");

            RuleFor(x => x.LeavingDate)
                .NotEmpty().WithMessage("LeavingDate is required.")
                .GreaterThan(x => x.ArrivingDate)
                .WithMessage("LeavingDate must be after ArrivingDate.");

            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");
        }
    }
}
