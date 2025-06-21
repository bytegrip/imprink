using FluentValidation;
using Imprink.Domain.Models;

namespace Imprink.Application.Validation.Users;

public class Auth0UserValidator : AbstractValidator<Auth0User>
{
    public Auth0UserValidator()
    {
        RuleFor(x => x.Sub)
            .NotEmpty();
        
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.Nickname)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}