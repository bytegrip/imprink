using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Imprink.Application.Commands.ProductVariants;

public class GetProductVariantsQuery : IRequest<IEnumerable<ProductVariantDto>>
{
    public Guid? ProductId { get; set; }
    public bool? IsActive { get; set; }
    public bool InStockOnly { get; set; } = false;
}

public class GetProductVariantsHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetProductVariantsHandler> logger)
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
        
        return mapper.Map<IEnumerable<ProductVariantDto>>(variants);
    }
}