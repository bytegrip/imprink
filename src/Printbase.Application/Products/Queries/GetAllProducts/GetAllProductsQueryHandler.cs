using MediatR;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Repositories;

namespace Printbase.Application.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetAllProductsQuery, AllProductsDto?>
{
    private readonly IProductRepository _productRepository = productRepository 
                                                             ?? throw new ArgumentNullException(nameof(productRepository));

    public async Task<AllProductsDto?> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(true, cancellationToken);
        
        var allProducts = new AllProductsDto
        {
            Products = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                TypeId = p.TypeId,
                TypeName = p.Type.Name,
                GroupName = p.Type.Group.Name,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive,
                Variants = request.IncludeVariants
                    ? p.Variants.Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        Color = v.Color,
                        Size = v.Size,
                        Price = v.Price,
                        Discount = v.Discount,
                        Stock = v.Stock,
                        SKU = v.SKU,
                        IsActive = v.IsActive
                    }).ToList()
                    : []
            }).ToList() 
        };

        return allProducts;
    }
}