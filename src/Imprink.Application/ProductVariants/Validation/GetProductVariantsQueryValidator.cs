using FluentValidation;
using Imprink.Application.ProductVariants.Commands;

namespace Imprink.Application.ProductVariants.Validation;

public class GetProductVariantsQueryValidator : AbstractValidator<GetProductVariantsQuery>
{
    public GetProductVariantsQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(Guid.Empty).When(x => x.ProductId.HasValue)
            .WithMessage("ProductId must be a valid GUID when provided.");
    }
}