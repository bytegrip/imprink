using Imprink.Application.Products.Create;
using Imprink.Application.Products.Delete;
using Imprink.Application.Products.Dtos;
using Imprink.Application.Products.Query;
using Imprink.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] bool? isActive = true,
        [FromQuery] bool? isCustomizable = null,
        [FromQuery] string sortBy = "Name",
        [FromQuery] string sortDirection = "ASC")
    {
        var filterParameters = new ProductFilterParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = searchTerm,
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            IsActive = isActive,
            IsCustomizable = isCustomizable,
            SortBy = sortBy,
            SortDirection = sortDirection
        };

        var query = new GetProductsQuery { FilterParameters = filterParameters };
        var result = await mediator.Send(query);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(CreateProduct), new { id = result.Id }, result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        var command = new DeleteProductCommand { Id = id };
        var result = await mediator.Send(command);
        
        if (!result)
            return NotFound();
            
        return NoContent();
    }
}