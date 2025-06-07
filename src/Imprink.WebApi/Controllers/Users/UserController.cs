using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public IActionResult SyncUser()
    {
        var claims = User.Claims;

        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        }

        return Ok("Claims logged to console.");
    }
}