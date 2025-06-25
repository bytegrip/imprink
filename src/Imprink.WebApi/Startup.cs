using FluentValidation;
using Imprink.Application;
using Imprink.Application.Commands.Products;
using Imprink.Application.Validation.Users;
using Imprink.Infrastructure.Database;
using Imprink.WebApi.Extensions;
using Imprink.WebApi.Filters;
using Imprink.WebApi.Middleware;
using Microsoft.EntityFrameworkCore;

namespace Imprink.WebApi;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddBusinessLogic();

        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        services.AddHttpContextAccessor();

        services.AddDatabaseContexts(builder.Configuration);
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProduct).Assembly));
        
        services.AddValidatorsFromAssembly(typeof(Auth0UserValidator).Assembly);
        
        services.AddJwtAuthentication(builder.Configuration);

        services.AddAuthorization();

        services.AddControllers(options => options.Filters.Add<ValidationActionFilter>());

        services.AddSwaggerWithJwtSecurity();
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.ApplyMigrations();

        app.UseGlobalExceptionHandling();
        app.UseRequestTiming();
        
        app.ConfigureEnvironment(env);

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}