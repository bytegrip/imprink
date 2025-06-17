using Imprink.Application.Products.Dtos;
using Imprink.Domain.Entities.Product;
using MediatR;

namespace Imprink.Application.Domains.ProductVariants;

public class GetProductVariantsQuery : IRequest<IEnumerable<ProductVariantDto>>
{
    public Guid? ProductId { get; set; }
    public bool? IsActive { get; set; }
    public bool InStockOnly { get; set; } = false;
}

public class GetProductVariantsHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProductVariantsQuery, IEnumerable<ProductVariantDto>>
{
    public async Task<IEnumerable<ProductVariantDto>> Handle(GetProductVariantsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ProductVariant> variants;

        if (request.ProductId.HasValue)
        {
            if (request.InStockOnly)
            {
                variants = await unitOfWork.ProductVariantRepository.GetInStockByProductIdAsync(request.ProductId.Value, cancellationToken);
            }
            else if (request.IsActive.HasValue && request.IsActive.Value)
            {
                variants = await unitOfWork.ProductVariantRepository.GetActiveByProductIdAsync(request.ProductId.Value, cancellationToken);
            }
            else
            {
                variants = await unitOfWork.ProductVariantRepository.GetByProductIdAsync(request.ProductId.Value, cancellationToken);
            }
        }
        else
        {
            variants = new List<ProductVariant>();
        }

        return variants.Select(pv => new ProductVariantDto
        {
            Id = pv.Id,
            ProductId = pv.ProductId,
            Size = pv.Size,
            Color = pv.Color,
            Price = pv.Price,
            ImageUrl = pv.ImageUrl,
            Sku = pv.Sku,
            StockQuantity = pv.StockQuantity,
            IsActive = pv.IsActive,
            Product = new ProductDto
            {
                Id = pv.Product.Id,
                Name = pv.Product.Name,
                Description = pv.Product.Description,
                BasePrice = pv.Product.BasePrice,
                IsCustomizable = pv.Product.IsCustomizable,
                IsActive = pv.Product.IsActive,
                ImageUrl = pv.Product.ImageUrl,
                CategoryId = pv.Product.CategoryId,
                CreatedAt = pv.Product.CreatedAt,
                ModifiedAt = pv.Product.ModifiedAt
            },
            CreatedAt = pv.CreatedAt,
            ModifiedAt = pv.ModifiedAt
        });
    }
}