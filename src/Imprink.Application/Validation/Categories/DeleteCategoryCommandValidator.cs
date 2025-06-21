using FluentValidation;
using Imprink.Application.Commands.Categories;

namespace Imprink.Application.Validation.Categories;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.")
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID.");
    }
}