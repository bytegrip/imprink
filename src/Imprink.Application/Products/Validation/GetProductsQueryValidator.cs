using FluentValidation;
using Imprink.Application.Domains.Products;
using Imprink.Application.Validation.Models;

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