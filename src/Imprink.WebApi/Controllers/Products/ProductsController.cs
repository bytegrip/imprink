using Imprink.Application.Products;
using Imprink.Application.Products.Dtos;
using Imprink.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<PagedResultDto<ProductDto>>> GetProducts(
        [FromQuery] ProductFilterParameters filterParameters)
    {
        var result = await mediator.Send(new GetProductsQuery { FilterParameters = filterParameters });
        return Ok(result);
    }
}