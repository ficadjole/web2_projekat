using FluentValidation;
using WebApi.DTOs.Checklist;

namespace WebApi.FluentValidations
{
    public class AddChecklistItemRequestValidator : AbstractValidator<AddChecklistItemRequest>
    {
        public AddChecklistItemRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Item name is required.")
                .MaximumLength(200).WithMessage("Item name cannot exceed 200 characters.");

            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");
        }
    }
}
