using Imprink.Application.Domains.ProductVariants;
using Imprink.Application.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/products/variants")]
public class ProductVariantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProductVariantDto>>> GetProductVariants(
        [FromQuery] GetProductVariantsQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductVariantDto>> CreateProductVariant(
        [FromBody] CreateProductVariantCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetProductVariants), new { id = result.Id }, result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductVariantDto>> UpdateProductVariant(
        Guid id,
        [FromBody] UpdateProductVariantCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
            
        return Ok(await mediator.Send(command));
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProductVariant(Guid id)
    {
        await mediator.Send(new DeleteProductVariantCommand { Id = id });
        return NoContent();
    }
}