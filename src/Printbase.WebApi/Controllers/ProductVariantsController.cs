using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;

namespace Printbase.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductVariantsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ProductVariantDto>> CreateProductVariant([FromBody] CreateProductVariantCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateProductVariant), new { id = result.Id }, result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductVariant(Guid id)
    {
        var command = new DeleteProductVariantCommand { Id = id };
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}