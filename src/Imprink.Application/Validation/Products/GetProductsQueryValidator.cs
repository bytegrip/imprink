using FluentValidation;
using Imprink.Application.Commands.Products;

namespace Imprink.Application.Validation.Products;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.FilterParameters)
            .NotNull()
            .SetValidator(new ProductFilterParametersValidator());
    }
}