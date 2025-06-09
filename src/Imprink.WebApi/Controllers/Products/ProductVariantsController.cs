using Imprink.Application.Products.Dtos;
using Imprink.Application.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/products/variants")]
public class ProductVariantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductVariantDto>>> GetProductVariants(
        [FromQuery] GetProductVariantsQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}