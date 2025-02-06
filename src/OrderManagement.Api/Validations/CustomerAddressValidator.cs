using FluentValidation;
using OrderManagement.Contracts.Customers;

namespace OrderManagement.Api.Validations;

public class CustomerAddressValidator : AbstractValidator<CustomerRequestDto.CustomerAddressDto>
{
    public CustomerAddressValidator()
    {
        RuleFor(a => a.Street)
            .NotEmpty().WithMessage("Street is required.");

        RuleFor(a => a.City)
            .NotEmpty().WithMessage("City is required.");

        RuleFor(a => a.PostalCode)
            .NotEmpty().WithMessage("Postal Code is required.");

        RuleFor(a => a.BuildingNr)
            .NotEmpty().WithMessage("Building Number is required.");

        RuleFor(a => a.Floor)
            .GreaterThanOrEqualTo(0).WithMessage("Floor number must be positive.");

        RuleFor(a => a.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");

        RuleFor(a => a.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
    }
}