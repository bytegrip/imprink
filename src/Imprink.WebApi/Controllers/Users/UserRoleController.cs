using System.Security.Claims;
using Imprink.Application.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/users/roles")]
public class UserRoleController(IMediator mediator) : ControllerBase
{
    //[Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyRoles()
    {
        var claims = User.Claims as Claim[] ?? User.Claims.ToArray();
        var sub = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        
        var myRoles = await mediator.Send(new GetUserRolesCommand(sub));
        
        return Ok(myRoles);
    }
}