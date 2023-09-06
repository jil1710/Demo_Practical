using Deixar.Domain.DTOs;
using FluentValidation;

namespace Deixar.Domain.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterUserModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(u => u.FirstName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);

            RuleFor(u => u.MiddleName)
                .MaximumLength(50);

            RuleFor(u => u.LastName)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50);

            RuleFor(u => u.EmailAddress)
                .MaximumLength(260)
                .EmailAddress();

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(128)
                .WithMessage("Password length must be between 6 to 128 characters.");

            RuleFor(u => u.Address)
                .MaximumLength(250).WithMessage("Address must contains less than 250 characters.");

            RuleFor(u => u.ContactNumber)
                .MinimumLength(10)
                .MaximumLength(10)
                .Matches("^\\d{10}$")
                .WithMessage("Contact number must contains 10 digits only.");

            RuleFor(u => u.Role)
                .NotEmpty()
                .NotNull().WithMessage("Please enter user role(HR, User)");
        }
    }
}
