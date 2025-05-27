using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;

namespace Printbase.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var command = new DeleteCategoryCommand { Id = id };
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}