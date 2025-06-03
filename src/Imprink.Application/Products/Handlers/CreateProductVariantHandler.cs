using Imprink.Application.Products.Commands;
using Imprink.Application.Products.Dtos;
using Imprink.Domain.Entities.Product;
using MediatR;

namespace Imprink.Application.Products.Handlers;

public class CreateProductVariantHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductVariantCommand, ProductVariantDto>
{
    public async Task<ProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var productVariant = new ProductVariant
            {
                ProductId = request.ProductId,
                Size = request.Size,
                Color = request.Color,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                IsActive = request.IsActive,
                Product = null!
            };

            var createdVariant = await unitOfWork.ProductVariantRepository.AddAsync(productVariant, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new ProductVariantDto
            {
                Id = createdVariant.Id,
                ProductId = createdVariant.ProductId,
                Size = createdVariant.Size,
                Color = createdVariant.Color,
                Price = createdVariant.Price,
                ImageUrl = createdVariant.ImageUrl,
                Sku = createdVariant.Sku,
                StockQuantity = createdVariant.StockQuantity,
                IsActive = createdVariant.IsActive,
                CreatedAt = createdVariant.CreatedAt,
                ModifiedAt = createdVariant.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}