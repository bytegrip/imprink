using Imprink.Domain.Repositories;

namespace Imprink.Application;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductVariantRepository ProductVariantRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    public IRoleRepository RoleRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IAddressRepository AddressRepository { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    Task<T> TransactAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
    Task TransactAsync(Func<Task> operation, CancellationToken cancellationToken = default);
}