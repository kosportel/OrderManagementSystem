using FluentValidation;
using OrderManagement.Contracts.Orders;

namespace OrderManagement.Api.Validations
{
    public class OrderValidator : AbstractValidator<OrderRequestDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be a valid ID.");

            RuleFor(o => o.AddressId) 
                .GreaterThan(0).WithMessage("AddressId must be a valid ID.");

            RuleFor(o => o.OrderTypeId)
                .IsInEnum().WithMessage("Order Type must be a valid ID.");

            RuleFor(o => o.Notes)
                .MaximumLength(255).WithMessage("Notes cannot exceed 255 characters.");

            RuleFor(o => o.OrderItems)
                .NotEmpty().WithMessage("At least one order item is required.")
                .Must(items => items.All(i => i.Quantity > 0))
                .WithMessage("Each order item must have a quantity greater than zero.");
        }
    }
}
