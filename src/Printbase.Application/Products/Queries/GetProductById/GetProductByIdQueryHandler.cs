using AutoMapper;
using MediatR;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Repositories;

namespace Printbase.Application.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler(IProductRepository productRepository, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository = productRepository 
                                                             ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, includeRelations: true, cancellationToken);
        
        if (product == null)
        {
            return null;
        }

        var productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            TypeId = product.TypeId,
            TypeName = product.Type.Name,
            GroupName = product.Type.Group.Name,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            IsActive = product.IsActive
        };

        if (request.IncludeVariants)
        {
            productDto.Variants = product.Variants
                .Select(v => new ProductVariantDto
                {
                    Id = v.Id,
                    Color = v.Color,
                    Size = v.Size,
                    Price = v.Price,
                    Discount = v.Discount,
                    Stock = v.Stock,
                    SKU = v.SKU,
                    IsActive = v.IsActive
                })
                .ToList();
        }

        return productDto;
    }
}