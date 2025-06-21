using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using MediatR;

namespace Imprink.Application.Commands.ProductVariants;

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

public class UpdateProductVariantHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateProductVariantCommand, ProductVariantDto>
{
    public async Task<ProductVariantDto> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            var existingVariant = await unitOfWork.ProductVariantRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (existingVariant == null)
                throw new NotFoundException($"Product variant with ID {request.Id} not found.");
            
            mapper.Map(request, existingVariant);

            var updatedVariant = await unitOfWork.ProductVariantRepository.UpdateAsync(existingVariant, cancellationToken);
            
            await unitOfWork.SaveAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return mapper.Map<ProductVariantDto>(updatedVariant);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}