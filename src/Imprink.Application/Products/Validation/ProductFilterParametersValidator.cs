using FluentValidation;
using Imprink.Domain.Models;

namespace Imprink.Application.Validation.Models;

public class ProductFilterParametersValidator : AbstractValidator<ProductFilterParameters>
{
    public ProductFilterParametersValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).WithMessage("PageNumber must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        
        RuleFor(x => x.SearchTerm)
            .Length(1, 100).WithMessage("Length must be between 1 and 100.")
            .When(x => !string.IsNullOrWhiteSpace(x.SearchTerm));

        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
            .WithMessage("MinPrice cannot be negative.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
            .WithMessage("MaxPrice cannot be negative.");

        RuleFor(x => x)
            .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
            .WithMessage("MinPrice cannot be greater than MaxPrice.");

        RuleFor(x => x.SortBy)
            .NotEmpty().WithMessage("SortBy is required.")
            .Must(value => AllowedSortColumns.Contains(value, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: Name, Price, CreatedDate.");

        RuleFor(x => x.SortDirection)
            .NotEmpty().WithMessage("SortDirection is required.")
            .Must(value => value.Equals("ASC", StringComparison.OrdinalIgnoreCase) 
                           || value.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be 'ASC' or 'DESC'.");
    }

    private static readonly string[] AllowedSortColumns = ["Name", "Price", "CreatedDate"];
}