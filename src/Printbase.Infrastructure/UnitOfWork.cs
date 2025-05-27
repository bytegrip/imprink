using Printbase.Application;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;

namespace Printbase.Infrastructure;

public class UnitOfWork(
    ApplicationDbContext context, 
    IProductRepository productRepository, 
    IProductVariantRepository productVariantRepository, 
    ICategoryRepository categoryRepository) : IUnitOfWork
{
    public IProductRepository ProductRepository => productRepository;
    public IProductVariantRepository ProductVariantRepository => productVariantRepository;
    public ICategoryRepository CategoryRepository => categoryRepository;

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await context.Database.RollbackTransactionAsync();
    }
}