using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Categories;

public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
{
    public bool? IsActive { get; set; }
    public bool RootCategoriesOnly { get; set; } = false;
}

public class GetCategories(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(
        GetCategoriesQuery request, 
        CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories;

        if (request.RootCategoriesOnly)
        {
            categories = await unitOfWork.CategoryRepository.GetRootCategoriesAsync(cancellationToken);
        }
        else if (request.IsActive.HasValue && request.IsActive.Value)
        {
            categories = await unitOfWork.CategoryRepository.GetActiveAsync(cancellationToken);
        }
        else
        {
            categories = await unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
        }

        return categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            SortOrder = c.SortOrder,
            IsActive = c.IsActive,
            ParentCategoryId = c.ParentCategoryId,
            CreatedAt = c.CreatedAt,
            ModifiedAt = c.ModifiedAt
        });
    }
}