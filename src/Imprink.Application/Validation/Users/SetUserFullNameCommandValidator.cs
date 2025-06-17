using FluentValidation;
using Imprink.Application.Domains.Users;

namespace Imprink.Application.Validation.Users;

public class SetUserFullNameCommandValidator : AbstractValidator<SetUserFullNameCommand>
{
    public SetUserFullNameCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .Length(1, 50).WithMessage("FirstName must be between 1 and 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .Length(1, 50).WithMessage("LastName must be between 1 and 50 characters.");
    }
}