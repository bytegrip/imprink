using Imprink.Application.Commands.Orders;
using Imprink.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/orders")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<OrderDto>> GetOrderById(
        Guid id, 
        [FromQuery] bool includeDetails = false, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetOrderByIdQuery 
        { 
            Id = id, 
            IncludeDetails = includeDetails 
        }, cancellationToken);
        
        if (result == null)
            return NotFound();
            
        return Ok(result);
    }
    
    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserId(
        string userId, 
        [FromQuery] bool includeDetails = false, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetOrdersByUserIdQuery 
        { 
            UserId = userId, 
            IncludeDetails = includeDetails 
        }, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("merchant/{merchantId}")]
    [Authorize(Roles = "Admin,Merchant")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByMerchantId(
        string merchantId, 
        [FromQuery] bool includeDetails = false, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetOrdersByMerchantIdQuery 
        { 
            MerchantId = merchantId, 
            IncludeDetails = includeDetails 
        }, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<OrderDto>> CreateOrder(
        [FromBody] CreateOrderCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
    }
}