using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;
using Printbase.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddOpenApi();
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
services.AddScoped<IProductGroupRepository, ProductGroupRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.Run();