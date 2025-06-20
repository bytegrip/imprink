using System.Security.Claims;
using Imprink.Application.Users.Services;
using Microsoft.AspNetCore.Http;

namespace Imprink.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? GetCurrentUserId()
    {
        return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
               ?? httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
    }
}