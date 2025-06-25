using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Domain.Entities;
using MediatR;

namespace Imprink.Application.Commands.Orders;

public class GetOrderByIdQuery : IRequest<OrderDto?>
{
    public Guid Id { get; set; }
    public bool IncludeDetails { get; set; }
}

public class GetOrderById(
    IUnitOfWork uw, 
    IMapper mapper) 
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(
        GetOrderByIdQuery request, 
        CancellationToken cancellationToken)
    {
        Order? order;
        
        if (request.IncludeDetails)
        {
            order = await uw.OrderRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);
        }
        else
        {
            order = await uw.OrderRepository.GetByIdAsync(request.Id, cancellationToken);
        }
        
        return order != null ? mapper.Map<OrderDto>(order) : null;
    }
}