using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers;

[ApiController]
[Route("/api/products/seed")]
public class SeedingController(Seeder seeder) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<bool>> Seed()
    {
        await seeder.SeedAsync();
        return Ok();
    }
}