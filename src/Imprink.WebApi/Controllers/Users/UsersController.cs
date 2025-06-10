using System.Security.Claims;
using Imprink.Application.Users;
using Imprink.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost("me/sync")]
    public async Task<IActionResult> SyncMyProfile()
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
        return Ok("User profile synchronized.");
    }
    
    [Authorize]
    [HttpGet("me/roles")]
    public async Task<IActionResult> GetMyRoles()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return Ok(await mediator.Send(new GetUserRolesCommand(sub)));
    }
    
    [Authorize]
    [HttpPut("me/phone")]
    public async Task<IActionResult> UpdateMyPhone([FromBody] SetUserPhoneCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [Authorize]
    [HttpPut("me/fullname")]
    public async Task<IActionResult> UpdateMyFullName([FromBody] SetUserFullNameCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}/roles/{roleId:guid}")]
    public async Task<IActionResult> AddUserRole(string userId, Guid roleId)
    {
        var command = new SetUserRoleCommand(userId, roleId);
        return Ok(await mediator.Send(command));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}/roles/{roleId:guid}")]
    public async Task<IActionResult> RemoveUserRole(string userId, Guid roleId)
    {
        var command = new DeleteUserRoleCommand(userId, roleId);
        return Ok(await mediator.Send(command));
    }
}