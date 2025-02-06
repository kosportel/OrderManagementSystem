using FluentValidation;
using OrderManagement.Contracts.Customers;

namespace OrderManagement.Api.Validations;
public class CustomerPhoneValidator : AbstractValidator<CustomerRequestDto.CustomerPhoneDto>
{
    public CustomerPhoneValidator()
    {
        RuleFor(p => p.Telephone)
            .NotEmpty().WithMessage("Telephone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");
    }
}