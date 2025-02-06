using FluentValidation;
using OrderManagement.Contracts.Users;

namespace OrderManagement.Api.Validations
{
    public class UserValidator : AbstractValidator<UserRequestDto>
    {
        public UserValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100).WithMessage("First name is required");
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100).WithMessage("Last name is required");
            RuleFor(u => u.Email).NotEmpty().EmailAddress().WithMessage("Email address is required");
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6).WithMessage("Password is required");
            RuleFor(u => u.RoleId).NotEmpty().WithMessage("Role is required");
        }
    }
}
