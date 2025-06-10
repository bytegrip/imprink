using System.Security.Claims;
using Imprink.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/users/roles")]
public class UserRoleController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyRoles()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return Ok(await mediator.Send(new GetUserRolesCommand(sub)));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("set")]
    public async Task<IActionResult> SetUserRole(SetUserRoleCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("unset")]
    public async Task<IActionResult> UnsetUserRole(DeleteUserRoleCommand command)
    {
        return Ok(await mediator.Send(command));
    }
}