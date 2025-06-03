using Imprink.Application.Products.Commands;
using Imprink.Application.Products.Dtos;
using Imprink.Domain.Entities.Product;
using MediatR;

namespace Imprink.Application.Products.Handlers;

public class CreateCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                SortOrder = request.SortOrder,
                IsActive = request.IsActive,
                ParentCategoryId = request.ParentCategoryId
            };

            var createdCategory = await unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                ImageUrl = createdCategory.ImageUrl,
                SortOrder = createdCategory.SortOrder,
                IsActive = createdCategory.IsActive,
                ParentCategoryId = createdCategory.ParentCategoryId,
                CreatedAt = createdCategory.CreatedAt,
                ModifiedAt = createdCategory.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}