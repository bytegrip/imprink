using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using MediatR;

namespace Imprink.Application.Commands.Products;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsCustomizable { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
}

public class UpdateProductHandler(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(
        UpdateProductCommand request, 
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var existingProduct = await unitOfWork.ProductRepository
                .GetByIdAsync(request.Id, cancellationToken);
            
            if (existingProduct == null) 
                throw new NotFoundException($"Product with ID {request.Id} not found.");

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.BasePrice = request.BasePrice;
            existingProduct.IsCustomizable = request.IsCustomizable;
            existingProduct.IsActive = request.IsActive;
            existingProduct.ImageUrl = request.ImageUrl;
            existingProduct.CategoryId = request.CategoryId;

            var updatedProduct = await unitOfWork.ProductRepository
                .UpdateAsync(existingProduct, cancellationToken);

            var categoryDto = new CategoryDto
            {
                Id = updatedProduct.Category.Id,
                Name = updatedProduct.Category.Name,
                Description = updatedProduct.Category.Description,
                ImageUrl = updatedProduct.Category.ImageUrl,
                SortOrder = updatedProduct.Category.SortOrder,
                IsActive = updatedProduct.Category.IsActive,
                ParentCategoryId = updatedProduct.Category.ParentCategoryId,
                CreatedAt = updatedProduct.Category.CreatedAt,
                ModifiedAt = updatedProduct.Category.ModifiedAt
            };

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                BasePrice = updatedProduct.BasePrice,
                IsCustomizable = updatedProduct.IsCustomizable,
                IsActive = updatedProduct.IsActive,
                ImageUrl = updatedProduct.ImageUrl,
                CategoryId = updatedProduct.CategoryId,
                Category = categoryDto,
                CreatedAt = updatedProduct.CreatedAt,
                ModifiedAt = updatedProduct.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}