using MediatR;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Product;

namespace Printbase.Application.Products.Handlers;

public class CreateCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
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
            await unitOfWork.CommitTransactionAsync();

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
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}