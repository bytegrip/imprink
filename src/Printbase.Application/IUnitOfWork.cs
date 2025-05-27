using Printbase.Domain.Repositories;

namespace Printbase.Application;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductVariantRepository ProductVariantRepository { get; }

    Task SaveAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}