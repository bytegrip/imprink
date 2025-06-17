using System.Security.Claims;
using FluentValidation;
using Imprink.Application;
using Imprink.Application.Domains.Products;
using Imprink.Application.Products;
using Imprink.Application.Services;
using Imprink.Application.Validation.Models;
using Imprink.Domain.Repositories;
using Imprink.Domain.Repositories.Orders;
using Imprink.Domain.Repositories.Products;
using Imprink.Domain.Repositories.Users;
using Imprink.Infrastructure;
using Imprink.Infrastructure.Database;
using Imprink.Infrastructure.Repositories.Orders;
using Imprink.Infrastructure.Repositories.Products;
using Imprink.Infrastructure.Repositories.Users;
using Imprink.Infrastructure.Services;
using Imprink.WebApi.Filters;
using Imprink.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Imprink.WebApi;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        services.AddScoped<Seeder>();
        
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).Assembly);
        });
        
        services.AddValidatorsFromAssembly(typeof(Auth0UserValidator).Assembly);
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
        {
            options.Authority = builder.Configuration["Auth0:Authority"];
            options.Audience = builder.Configuration["Auth0:Audience"];

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

        services.AddAuthorization();

        services.AddControllers(options =>
            {
                options.Filters.Add<ValidationActionFilter>();
            });
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Imprink API",
                Version = "v1",
            });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    []
                }
            });
        });
    }

    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (app is WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                dbContext.Database.Migrate();
                Console.WriteLine("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
            }
        }

        app.UseGlobalExceptionHandling();
        app.UseRequestTiming();
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}