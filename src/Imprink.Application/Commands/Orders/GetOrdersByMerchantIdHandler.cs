using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Orders;

public class GetOrdersByMerchantIdQuery : IRequest<IEnumerable<OrderDto>>
{
    public string MerchantId { get; set; } = null!;
    public bool IncludeDetails { get; set; }
}

public class GetOrdersByMerchantIdHandler(IUnitOfWork uw, IMapper mapper) : IRequestHandler<GetOrdersByMerchantIdQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersByMerchantIdQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders;
        
        if (request.IncludeDetails)
        {
            orders = await uw.OrderRepository.GetByMerchantIdWithDetailsAsync(request.MerchantId, cancellationToken);
        }
        else
        {
            orders = await uw.OrderRepository.GetByMerchantIdAsync(request.MerchantId, cancellationToken);
        }
        
        return mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}