using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.WebApi.Extensions;

public static class StartupMigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        if (app is not WebApplication application) return;
        
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
}