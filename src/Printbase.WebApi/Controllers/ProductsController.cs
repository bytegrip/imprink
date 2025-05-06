using MediatR;
using Microsoft.AspNetCore.Mvc;
using Printbase.Application.Products.Commands.CreateProduct;
using Printbase.Application.Products.Queries.GetAllProducts;
using Printbase.Application.Products.Queries.GetProductById;

namespace Printbase.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllProducts()
    {
        var query = new GetAllProductsQuery();
        var result = await _mediator.Send(query);

        if (result == null) return NotFound();
        return Ok(result);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id, [FromQuery] bool includeVariants = true)
    {
        var query = new GetProductByIdQuery(id, includeVariants);
        var result = await _mediator.Send(query);

        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}