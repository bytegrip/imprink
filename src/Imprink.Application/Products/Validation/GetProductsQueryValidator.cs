using FluentValidation;
using Imprink.Application.Products.Commands;

namespace Imprink.Application.Products.Validation;

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(x => x.FilterParameters)
            .NotNull()
            .SetValidator(new ProductFilterParametersValidator());
    }
}