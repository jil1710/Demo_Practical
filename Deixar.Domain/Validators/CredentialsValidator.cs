using Deixar.Domain.DTOs;
using FluentValidation;

namespace Deixar.Domain.Validators
{
    public class CredentialsValidator : AbstractValidator<Credentials>
    {
        public CredentialsValidator()
        {
            RuleFor(c => c.EmailAddress)
                .MaximumLength(260)
                .EmailAddress();

            RuleFor(c => c.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(128)
                .WithMessage("Password length must be between 6 to 128 characters.");
        }
    }
}
