using MediatR;
using Printbase.Application.Products.Dtos;
using Printbase.Application.Products.Queries;

namespace Printbase.Application.Products.Handlers;

public class GetCategoriesHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Product.Category> categories;

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