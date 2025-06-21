using FluentValidation;
using Imprink.Application.Commands.ProductVariants;

namespace Imprink.Application.Validation.ProductVariants;

public class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .NotEqual(Guid.Empty).WithMessage("ProductId must be a valid GUID.");

        RuleFor(x => x.Size)
            .NotEmpty().WithMessage("Size is required.")
            .Length(1, 50).WithMessage("Size must be between 1 and 50 characters.");

        RuleFor(x => x.Color)
            .Length(1, 50).WithMessage("Color must be between 1 and 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Color));

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .Length(1, 50).WithMessage("SKU must be between 1 and 50 characters.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("StockQuantity cannot be negative.");
    }

    private static bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}