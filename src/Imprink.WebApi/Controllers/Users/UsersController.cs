using System.Security.Claims;
using AutoMapper;
using Imprink.Application.Domains.Users;
using Imprink.Application.Users.Dtos;
using Imprink.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imprink.WebApi.Controllers.Users;

[ApiController]
[Route("/api/users")]
public class UsersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    [Authorize]
    [HttpPost("me/sync")]
    public async Task<IActionResult> SyncMyProfile()
    {
        var auth0User = mapper.Map<Auth0User>(User);
        
        await mediator.Send(new SyncUserCommand(auth0User));
        return Ok("Synced");
    }
    
    [Authorize]
    [HttpGet("me/roles")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetMyRoles()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        return Ok(await mediator.Send(new GetUserRolesCommand(sub)));
    }
    
    [Authorize]
    [HttpPut("me/phone")]
    public async Task<ActionResult<UserDto?>> UpdateMyPhone([FromBody] SetUserPhoneCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [Authorize]
    [HttpPut("me/fullname")]
    public async Task<ActionResult<UserDto?>> UpdateMyFullName([FromBody] SetUserFullNameCommand command)
    {
        return Ok(await mediator.Send(command));
    }
    
    [Authorize]
    [HttpGet("roles")]
    public async Task<ActionResult<UserRoleDto?>> GetAllRoles()
    {
        var command = new GetAllRolesCommand();
        return Ok(await mediator.Send(command));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{userId}/roles/{roleId:guid}")]
    public async Task<ActionResult<UserRoleDto?>> AddUserRole(string userId, Guid roleId)
    {
        var command = new SetUserRoleCommand(userId, roleId);
        return Ok(await mediator.Send(command));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}/roles/{roleId:guid}")]
    public async Task<ActionResult<UserRoleDto?>> RemoveUserRole(string userId, Guid roleId)
    {
        var command = new DeleteUserRoleCommand(userId, roleId);
        return Ok(await mediator.Send(command));
    }
}