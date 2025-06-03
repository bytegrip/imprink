using Imprink.Application.Products.Commands;
using Imprink.Application.Products.Dtos;
using Imprink.Domain.Entities.Product;
using MediatR;

namespace Imprink.Application.Products.Handlers;

public class CreateProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
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
            
            await unitOfWork.CommitTransactionAsync(cancellationToken);

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
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}