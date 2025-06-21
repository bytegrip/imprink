using System.Security.Claims;
using Imprink.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Imprink.WebApi.Extensions;

public static class StartupSecurityExtensions
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = configuration["Auth0:Authority"];
                options.Audience = configuration["Auth0:Audience"];

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
                        var userId = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                     ?? context.Principal?.FindFirst("sub")?.Value;

                        if (string.IsNullOrEmpty(userId)) return Task.CompletedTask;
                        var identity = context.Principal!.Identity as ClaimsIdentity;

                        var roles = (
                            from ur in dbContext?.UserRole
                            join r in dbContext?.Roles on ur.RoleId equals r.Id
                            where ur.UserId == userId
                            select r.RoleName).ToList();

                        foreach (var role in roles) identity!.AddClaim(new Claim(ClaimTypes.Role, role));
                        identity!.AddClaim(new Claim(ClaimTypes.Role, "User"));
                    
                        return Task.CompletedTask;
                    }
                };
            });
    }
}