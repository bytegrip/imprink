using FluentValidation;
using Imprink.Application.Commands.Orders;

namespace Imprink.Application.Validation.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity cannot exceed 1000 items per order.");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.ProductVariantId)
            .NotEmpty()
            .WithMessage("Product variant ID is required.");

        RuleFor(x => x.AddressId)
            .NotEmpty()
            .WithMessage("Address ID is required.");

        RuleFor(x => x.CustomizationImageUrl)
            .MaximumLength(2048)
            .WithMessage("Customization image URL must not exceed 2048 characters.")
            .Must(BeValidUrl)
            .When(x => !string.IsNullOrEmpty(x.CustomizationImageUrl))
            .WithMessage("Customization image URL must be a valid URL.");

        RuleFor(x => x.CustomizationDescription)
            .MaximumLength(2000)
            .WithMessage("Customization description must not exceed 2000 characters.");

        RuleFor(x => x.OriginalImageUrls)
            .Must(HaveValidImageUrls)
            .When(x => x.OriginalImageUrls != null && x.OriginalImageUrls.Length > 0)
            .WithMessage("All original image URLs must be valid URLs.")
            .Must(x => x == null || x.Length <= 10)
            .WithMessage("Cannot have more than 10 original images.");

        RuleForEach(x => x.OriginalImageUrls)
            .MaximumLength(2048)
            .WithMessage("Each original image URL must not exceed 2048 characters.")
            .Must(BeValidUrl)
            .WithMessage("Each original image URL must be a valid URL.");
    }

    private static bool BeValidUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var result) 
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static bool HaveValidImageUrls(string[]? urls)
    {
        if (urls == null || urls.Length == 0)
            return true;

        return urls.All(url => !string.IsNullOrEmpty(url) && BeValidUrl(url));
    }
}