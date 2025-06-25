using Imprink.Application.Commands.Products;
using Imprink.Application.Dtos;
using Imprink.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProducts(
        [FromQuery] ProductFilterParameters filterParameters, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductsQuery { FilterParameters = filterParameters}, cancellationToken);
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProductById(
        Guid id,  
        CancellationToken cancellationToken)
    {
        var result = await mediator
            .Send(new GetProductByIdQuery { ProductId = id}, cancellationToken);
        
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> CreateProduct(
        [FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(
        Guid id,
        [FromBody] UpdateProduct command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
            
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        await mediator.Send(new DeleteProductCommand { Id = id });
        return NoContent();
    }
}