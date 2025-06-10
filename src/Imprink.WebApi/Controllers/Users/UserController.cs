using System.Security.Claims;
using Imprink.Application.Users;
using Imprink.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/users")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost("sync")]
    public async Task<IActionResult> Sync()
    {
        var claims = User.Claims as Claim[] ?? User.Claims.ToArray();
        
        var auth0User = new Auth0User
        {
            Sub = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Name = claims.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
            Nickname = claims.FirstOrDefault(c => c.Type == "nickname")?.Value ?? string.Empty,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty,
            EmailVerified = bool.TryParse(claims.FirstOrDefault(c => c.Type == "email_verified")?.Value, out var emailVerified) && emailVerified
        };
        
        await mediator.Send(new SyncUserCommand(auth0User));
        return Ok("User Synced.");
    }
}