using MediatR;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Products;
using Printbase.Domain.Repositories;

namespace Printbase.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(
    IProductRepository productRepository,
    IProductVariantRepository variantRepository,
    IProductTypeRepository typeRepository)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    private readonly IProductVariantRepository _variantRepository = variantRepository ?? throw new ArgumentNullException(nameof(variantRepository));
    private readonly IProductTypeRepository _typeRepository = typeRepository ?? throw new ArgumentNullException(nameof(typeRepository));

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productType = await _typeRepository.GetByIdAsync(request.TypeId, includeRelations: true, cancellationToken);
        if (productType == null)
        {
            throw new ArgumentException($"Product type with ID {request.TypeId} not found");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            TypeId = request.TypeId,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdProduct = await _productRepository.AddAsync(product, cancellationToken);

        var productVariants = new List<ProductVariant>();
        if (request.Variants != null && request.Variants.Count != 0)
        {
            foreach (var variant in request.Variants.Select(variantDto => new ProductVariant
                     {
                         Id = Guid.NewGuid(),
                         ProductId = createdProduct.Id,
                         Color = variantDto.Color,
                         Size = variantDto.Size,
                         Price = variantDto.Price,
                         Discount = variantDto.Discount,
                         Stock = variantDto.Stock,
                         SKU = variantDto.SKU ?? GenerateSku(createdProduct.Name, variantDto.Color, variantDto.Size),
                         CreatedAt = DateTime.UtcNow,
                         IsActive = true
                     }))
            {
                var createdVariant = await _variantRepository.AddAsync(variant, cancellationToken);
                productVariants.Add(createdVariant);
            }
        }

        var productDto = new ProductDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            TypeId = createdProduct.TypeId,
            TypeName = productType.Name,
            GroupName = productType.Group.Name,
            CreatedAt = createdProduct.CreatedAt,
            IsActive = createdProduct.IsActive,
            Variants = productVariants.Select(v => new ProductVariantDto
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
        };

        return productDto;
    }

    public static string GenerateSku(string productName, string? color, string? size)
    {
        var prefix = productName.Length >= 3 ? productName[..3].ToUpper() : productName.ToUpper();
        var colorPart = !string.IsNullOrEmpty(color) ? color[..Math.Min(3, color.Length)].ToUpper() : "XXX";
        var sizePart = !string.IsNullOrEmpty(size) ? size.ToUpper() : "OS";
        var randomPart = new Random().Next(100, 999).ToString();

        return $"{prefix}-{colorPart}-{sizePart}-{randomPart}";
    }
}