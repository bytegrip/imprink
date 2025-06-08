using System.Security.Claims;
using Imprink.Application.Users;
using Imprink.Domain.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/users")]
public class UserController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost("sync")]
    public async Task<IActionResult> Sync()
    {
        var enumerable = User.Claims as Claim[] ?? User.Claims.ToArray();
        
        var auth0User = new Auth0User
        {
            Sub = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
            Name = enumerable.FirstOrDefault(c => c.Type == "name")?.Value ?? string.Empty,
            Nickname = enumerable.FirstOrDefault(c => c.Type == "nickname")?.Value ?? string.Empty,
            Email = enumerable.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty,
            EmailVerified = bool.TryParse(enumerable.FirstOrDefault(c => c.Type == "email_verified")?.Value, out var emailVerified) && emailVerified
        };
        
        await mediator.Send(new SyncUserCommand(auth0User));

        return Ok("User Synced.");
    }
}