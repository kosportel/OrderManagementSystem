using FluentValidation;
using OrderManagement.Contracts.Orders;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Api.Validations
{
    public class UpdateDeliveryStatusDtoValidator : AbstractValidator<UpdateDeliveryStatusDto>
    {
        public UpdateDeliveryStatusDtoValidator()
        {
            RuleFor(x => x.Notes)
                .MaximumLength(255).WithMessage("Notes cannot exceed 255 characters.");
        }
    }
}
