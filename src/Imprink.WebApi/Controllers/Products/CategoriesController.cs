using Imprink.Application.Products.Dtos;
using Imprink.Application.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("api/products/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] GetCategoriesQuery query)
    {
        return Ok(await mediator.Send(query));
    }
}