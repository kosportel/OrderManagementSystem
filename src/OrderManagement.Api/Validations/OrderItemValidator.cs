using FluentValidation;
using OrderManagement.Contracts.Orders;

namespace OrderManagement.Api.Validations
{
    public class OrderItemValidator : AbstractValidator<OrderItemRequestDto>
    {
        public OrderItemValidator()
        {
            RuleFor(oi => oi.MenuItemId)
                .GreaterThan(0).WithMessage("MenuItemId must be a valid ID.");

            RuleFor(oi => oi.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(oi => oi.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(oi => oi.Notes)
                .MaximumLength(255).WithMessage("Notes cannot exceed 255 characters.");
        }
    }
}
