using Deixar.Domain.Entities;
using FluentValidation;

namespace Deixar.Domain.Validators
{
    public class LeaveValidator : AbstractValidator<Leave>
    {
        public LeaveValidator()
        {
            RuleFor(l => l.LeaveDate).NotNull().NotEmpty();
            RuleFor(l => l.Reason).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(l => l.Notes).MaximumLength(400);
        }
    }
}
