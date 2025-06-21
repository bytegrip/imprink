using FluentValidation;
using Imprink.Application.Commands.ProductVariants;

namespace Imprink.Application.Validation.ProductVariants;

public class GetProductVariantsQueryValidator : AbstractValidator<GetProductVariantsQuery>
{
    public GetProductVariantsQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(Guid.Empty).When(x => x.ProductId.HasValue)
            .WithMessage("ProductId must be a valid GUID when provided.");
    }
}