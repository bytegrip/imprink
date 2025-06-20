using FluentValidation;
using Imprink.Application.Categories.Commands;

namespace Imprink.Application.Categories.Validation;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(1, 500).WithMessage("Description must be between 1 and 500 characters.");

        RuleFor(x => x.ImageUrl)
            .Must(BeValidUrl).When(x => !string.IsNullOrWhiteSpace(x.ImageUrl))
            .WithMessage("ImageUrl must be a valid URL.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("SortOrder cannot be negative.");

        RuleFor(x => x.ParentCategoryId)
            .NotEqual(Guid.Empty).When(x => x.ParentCategoryId.HasValue)
            .WithMessage("ParentCategoryId must be a valid GUID.");
    }

    private static bool BeValidUrl(string? url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}