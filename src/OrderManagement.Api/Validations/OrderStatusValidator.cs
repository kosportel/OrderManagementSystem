using FluentValidation;
using OrderManagement.Contracts.Orders;

namespace OrderManagement.Api.Validations
{
    public class OrderStatusValidator : AbstractValidator<UpdateOrderStatusRequestDto>
    {
        public OrderStatusValidator()
        {
            RuleFor(os => os.OrderStatusId)
                .IsInEnum().WithMessage("OrderStatusId must be a valid ID.");
        }
    }
}
