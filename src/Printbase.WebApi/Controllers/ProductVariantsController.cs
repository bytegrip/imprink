using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;
using Printbase.Application.Products.Queries;

namespace Printbase.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductVariantDto>>> GetProductVariants(
        [FromQuery] Guid? productId = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] bool inStockOnly = false)
    {
        var query = new GetProductVariantsQuery
        {
            ProductId = productId,
            IsActive = isActive,
            InStockOnly = inStockOnly
        };
        
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductVariantDto>> CreateProductVariant([FromBody] CreateProductVariantCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateProductVariant), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProductVariant(Guid id)
    {
        var command = new DeleteProductVariantCommand { Id = id };
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}