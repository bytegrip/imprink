using Imprink.Application.Commands.Addresses;
using Imprink.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/addresses")]
public class AddressesController(IMediator mediator) : ControllerBase
{
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AddressDto?>>> GetMyAddresses(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetMyAddressesQuery(), cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("user/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddressesByUserId(
        string userId, 
        [FromQuery] bool activeOnly = false,
        [FromQuery] string? addressType = null, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetAddressesByUserIdQuery 
        { 
            UserId = userId, 
            ActiveOnly = activeOnly,
            AddressType = addressType 
        }, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<AddressDto>> CreateAddress(
        [FromBody] CreateAddressCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateAddress), new { id = result.Id }, result);
    }
}