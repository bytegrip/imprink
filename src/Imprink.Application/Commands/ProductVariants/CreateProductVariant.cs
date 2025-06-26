using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.ProductVariants;

public class CreateProductVariantCommand : IRequest<ProductVariantDto>
{
    public Guid ProductId { get; set; }
    public string Size { get; set; } = null!;
    public string? Color { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Sku { get; set; } = null!;
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateProductVariant(
    IUnitOfWork unitOfWork, 
    IMapper mapper)
    : IRequestHandler<CreateProductVariantCommand, ProductVariantDto>
{
    public async Task<ProductVariantDto> Handle(
        CreateProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var productVariant = mapper.Map<ProductVariant>(request);
            
            productVariant.Product = null!;

            var createdVariant = await unitOfWork.ProductVariantRepository
                .AddAsync(productVariant, cancellationToken);
            
            await unitOfWork.SaveAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return mapper.Map<ProductVariantDto>(createdVariant);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}