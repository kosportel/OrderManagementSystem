using FluentValidation;
using OrderManagement.Contracts.Customers;

namespace OrderManagement.Api.Validations;

public class CustomerRequestValidator : AbstractValidator<CustomerRequestDto>
{
    public CustomerRequestValidator()
    {
        RuleFor(c => c.UserId)
            .GreaterThan(0).WithMessage("User is required");

        RuleFor(c => c.Addresses)
            .NotEmpty().WithMessage("At least one address is required.");

        RuleForEach(c => c.Addresses).SetValidator(new CustomerAddressValidator());

        RuleFor(c => c.Phones)
            .NotEmpty().WithMessage("At least one phone number is required.");

        RuleForEach(c => c.Phones).SetValidator(new CustomerPhoneValidator());
    }
}
