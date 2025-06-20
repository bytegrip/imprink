using FluentValidation;
using Imprink.Application.Products.Commands;

namespace Imprink.Application.Products.Validation;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
    }
}