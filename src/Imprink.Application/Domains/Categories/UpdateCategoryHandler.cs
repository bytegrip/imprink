using Imprink.Application.Exceptions;
using Imprink.Application.Products.Dtos;
using MediatR;

namespace Imprink.Application.Domains.Categories;

public class UpdateCategoryCommand : IRequest<CategoryDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
}

public class UpdateCategoryHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var existingCategory = await unitOfWork.CategoryRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (existingCategory == null)
            {
                throw new NotFoundException($"Category with ID {request.Id} not found.");
            }

            existingCategory.Name = request.Name;
            existingCategory.Description = request.Description;
            existingCategory.ImageUrl = request.ImageUrl;
            existingCategory.SortOrder = request.SortOrder;
            existingCategory.IsActive = request.IsActive;
            existingCategory.ParentCategoryId = request.ParentCategoryId;

            var updatedCategory = await unitOfWork.CategoryRepository.UpdateAsync(existingCategory, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Description = updatedCategory.Description,
                ImageUrl = updatedCategory.ImageUrl,
                SortOrder = updatedCategory.SortOrder,
                IsActive = updatedCategory.IsActive,
                ParentCategoryId = updatedCategory.ParentCategoryId,
                CreatedAt = updatedCategory.CreatedAt,
                ModifiedAt = updatedCategory.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}