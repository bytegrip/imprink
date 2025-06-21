using FluentValidation;
using Imprink.Application.Commands.Users;

namespace Imprink.Application.Validation.Users;

public class SetUserPhoneCommandValidator : AbstractValidator<SetUserPhoneCommand>
{
    public SetUserPhoneCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("PhoneNumber must be a valid phone number format.");
    }
}