using Imprink.Application.Categories.Commands;
using Imprink.Application.Categories.Dtos;
using Imprink.Application.Products.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("api/products/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        [FromQuery] GetCategoriesQuery query)
    {
        return Ok(await mediator.Send(query));
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> CreateCategory(
        [FromBody] CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCategories), new { id = result.Id }, result);
    }
    
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(
        Guid id,
        [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch");
            
        return Ok(await mediator.Send(command));
    }
    
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await mediator.Send(new DeleteCategoryCommand { Id = id });
        return NoContent();
    }
}