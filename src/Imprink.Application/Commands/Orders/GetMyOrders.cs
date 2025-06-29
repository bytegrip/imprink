using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Orders;

public class GetOrdersByUserIdQuery : IRequest<IEnumerable<OrderDto>>
{
    public string UserId { get; set; } = null!;
    public bool IncludeDetails { get; set; }
}

public class GetOrdersByUserId(
    IUnitOfWork uw, 
    IMapper mapper) 
    : IRequestHandler<GetOrdersByUserIdQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(
        GetOrdersByUserIdQuery request, 
        CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders;
        
        if (request.IncludeDetails)
        {
            orders = await uw.OrderRepository
                .GetByUserIdWithDetailsAsync(request.UserId, cancellationToken);
        }
        else
        {
            orders = await uw.OrderRepository
                .GetByUserIdAsync(request.UserId, cancellationToken);
        }
        
        return mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}