using Imprink.Application;
using Imprink.Application.Products.Handlers;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure;
using Imprink.Infrastructure.Database;
using Imprink.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Imprink.WebApi;

public static class Startup
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateProductHandler).Assembly);
        });

        services.AddControllers();
        services.AddSwaggerGen();
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
        
        // if (env.IsDevelopment())
        // {
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        //     app.UseDeveloperExceptionPage();
        // }
        // else
        // {
        //     app.UseExceptionHandler("/Error");
        //     app.UseHsts();
        //     app.UseHttpsRedirection();
        // }
        
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}