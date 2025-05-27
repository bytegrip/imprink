using MediatR;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Product;

namespace Printbase.Application.Products.Handlers;

public class CreateProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
        try
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice,
                IsCustomizable = request.IsCustomizable,
                IsActive = request.IsActive,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                Category = null!
            };

            var createdProduct = await unitOfWork.ProductRepository.AddAsync(product, cancellationToken);

            var categoryDto = new CategoryDto
            {
                Id = createdProduct.Category.Id,
                Name = createdProduct.Category.Name,
                Description = createdProduct.Category.Description,
                ImageUrl = createdProduct.Category.ImageUrl,
                SortOrder = createdProduct.Category.SortOrder,
                IsActive = createdProduct.Category.IsActive,
                ParentCategoryId = createdProduct.Category.ParentCategoryId,
                CreatedAt = createdProduct.Category.CreatedAt,
                ModifiedAt = createdProduct.Category.ModifiedAt
            };
            
            await unitOfWork.CommitTransactionAsync();

            return new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                BasePrice = createdProduct.BasePrice,
                IsCustomizable = createdProduct.IsCustomizable,
                IsActive = createdProduct.IsActive,
                ImageUrl = createdProduct.ImageUrl,
                CategoryId = createdProduct.CategoryId,
                Category = categoryDto,
                CreatedAt = createdProduct.CreatedAt,
                ModifiedAt = createdProduct.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}