using Microsoft.EntityFrameworkCore;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext>? options) : DbContext(options)
{
}