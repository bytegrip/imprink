using FluentValidation;
using Imprink.Application.Commands.ProductVariants;

namespace Imprink.Application.Validation.ProductVariants;

public class DeleteProductVariantCommandValidator : AbstractValidator<DeleteProductVariantCommand>
{
    public DeleteProductVariantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
    }
}