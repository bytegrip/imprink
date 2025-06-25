using Imprink.Application.Dtos;
using MediatR;

namespace Imprink.Application.Commands.Products;

public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public Guid ProductId { get; set; }
}

public class GetProductById(
    IUnitOfWork unitOfWork) 
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var product = await unitOfWork.ProductRepository
            .GetByIdAsync(request.ProductId, cancellationToken);
        
        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BasePrice = product.BasePrice,
            IsCustomizable = product.IsCustomizable,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            Category = new CategoryDto
            {
                Id = product.Category.Id,
                Name = product.Category.Name,
                Description = product.Category.Description,
                ImageUrl = product.Category.ImageUrl,
                SortOrder = product.Category.SortOrder,
                IsActive = product.Category.IsActive,
                ParentCategoryId = product.Category.ParentCategoryId,
                CreatedAt = product.Category.CreatedAt,
                ModifiedAt = product.Category.ModifiedAt
            },
            CreatedAt = product.CreatedAt,
            ModifiedAt = product.ModifiedAt
        };
    }
}