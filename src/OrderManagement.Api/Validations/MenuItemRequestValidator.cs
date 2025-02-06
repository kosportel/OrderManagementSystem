using FluentValidation;
using OrderManagement.Contracts.MenuItem;

namespace OrderManagement.Api.Validations
{
    public class MenuItemRequestValidator : AbstractValidator<MenuItemRequestDto>
    {
        public MenuItemRequestValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(m => m.Ingredients)
                .NotEmpty().WithMessage("Ingredients are required.")
                .MaximumLength(500).WithMessage("Ingredients cannot exceed 500 characters.");

            RuleFor(m => m.Allergies)
                .NotEmpty().WithMessage("Allergy information is required.")
                .MaximumLength(200).WithMessage("Allergies cannot exceed 200 characters.");

            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(m => m.ExpectedPrepMinutes)
                .GreaterThan(0).WithMessage("Expected preparation time must be greater than zero.");
        }
    }
}
