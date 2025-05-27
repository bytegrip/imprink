using MediatR;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;
using Printbase.Domain.Entities.Product;

namespace Printbase.Application.Products.Handlers;

public class CreateProductVariantHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductVariantCommand, ProductVariantDto>
{
    public async Task<ProductVariantDto> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync();
        
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
            await unitOfWork.CommitTransactionAsync();

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
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}