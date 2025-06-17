using Imprink.Application.Exceptions;
using Imprink.Application.Products.Dtos;
using MediatR;

namespace Imprink.Application.Domains.ProductVariants;

public class UpdateProductVariantCommand : IRequest<ProductVariantDto>
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Size { get; set; } = null!;
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Sku { get; set; } = null!;
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateProductVariantHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductVariantCommand, ProductVariantDto>
{
    public async Task<ProductVariantDto> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var existingVariant = await unitOfWork.ProductVariantRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (existingVariant == null)
            {
                throw new NotFoundException($"Product variant with ID {request.Id} not found.");
            }

            existingVariant.ProductId = request.ProductId;
            existingVariant.Size = request.Size;
            existingVariant.Color = request.Color;
            existingVariant.Price = request.Price;
            existingVariant.ImageUrl = request.ImageUrl;
            existingVariant.Sku = request.Sku;
            existingVariant.StockQuantity = request.StockQuantity;
            existingVariant.IsActive = request.IsActive;

            var updatedVariant = await unitOfWork.ProductVariantRepository.UpdateAsync(existingVariant, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return new ProductVariantDto
            {
                Id = updatedVariant.Id,
                ProductId = updatedVariant.ProductId,
                Size = updatedVariant.Size,
                Color = updatedVariant.Color,
                Price = updatedVariant.Price,
                ImageUrl = updatedVariant.ImageUrl,
                Sku = updatedVariant.Sku,
                StockQuantity = updatedVariant.StockQuantity,
                IsActive = updatedVariant.IsActive,
                CreatedAt = updatedVariant.CreatedAt,
                ModifiedAt = updatedVariant.ModifiedAt
            };
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}