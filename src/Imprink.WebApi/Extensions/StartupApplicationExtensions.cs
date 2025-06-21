using Imprink.Application;
using Imprink.Application.Services;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure;
using Imprink.Infrastructure.Database;
using Imprink.Infrastructure.Repositories;
using Imprink.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Imprink.WebApi.Extensions;

public static class StartupApplicationExtensions
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
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
    }

    public static void AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
    }   
}