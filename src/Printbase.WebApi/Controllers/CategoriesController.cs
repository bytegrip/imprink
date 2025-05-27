using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printbase.Application.Products.Commands;
using Printbase.Application.Products.Dtos;
using Printbase.Application.Products.Queries;

namespace Printbase.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
        [FromQuery] bool? isActive = null,
        [FromQuery] bool rootCategoriesOnly = false)
    {
        var query = new GetCategoriesQuery
        {
            IsActive = isActive,
            RootCategoriesOnly = rootCategoriesOnly
        };
        
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
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