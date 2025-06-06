using System.Security.Claims;
using Imprink.Domain.Common.Models;
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

        var enumerable = claims as Claim[] ?? claims.ToArray();
        var user = new Auth0User
        {
            Sub = enumerable.FirstOrDefault(c => c.Type == "sub")?.Value ?? "",
            Name = enumerable.FirstOrDefault(c => c.Type == "name")?.Value ?? "",
            Nickname = enumerable.FirstOrDefault(c => c.Type == "nickname")?.Value ?? "",
            Email = enumerable.FirstOrDefault(c => c.Type == "email")?.Value ?? "",
            EmailVerified = enumerable.FirstOrDefault(c => c.Type == "email_verified")?.Value == "true"
        };
        
        return Ok(user);
    }
}