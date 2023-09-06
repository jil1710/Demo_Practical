using Deixar.Domain.Entities;
using FluentValidation;

namespace Deixar.Domain.Validators;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator()
    {
        RuleFor(r => r.RoleName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(50);
    }
}
