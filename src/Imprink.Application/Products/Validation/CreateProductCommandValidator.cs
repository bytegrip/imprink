using FluentValidation;
using Imprink.Application.Products.Commands;

namespace Imprink.Application.Products.Validation;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

        RuleFor(x => x.Description)
            .Length(1, 1000).WithMessage("Description must be between 1 and 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.BasePrice)
            .GreaterThan(0).WithMessage("BasePrice must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.CategoryId)
            .NotEqual(Guid.Empty).When(x => x.CategoryId.HasValue)
            .WithMessage("CategoryId must be a valid GUID.");
    }

    private static bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}