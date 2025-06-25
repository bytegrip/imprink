using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Categories;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
    public Guid? ParentCategoryId { get; set; }
}

public class CreateCategory(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken)
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

            var createdCategory = await unitOfWork
                .CategoryRepository.AddAsync(category, cancellationToken);
            
            await unitOfWork.SaveAsync(cancellationToken);
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