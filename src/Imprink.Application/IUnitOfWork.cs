using Imprink.Domain.Repositories;
using Imprink.Domain.Repositories.Products;
using Imprink.Domain.Repositories.Users;

namespace Imprink.Application;

public interface IUnitOfWork
{
    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IProductVariantRepository ProductVariantRepository { get; }
    public IUserRepository UserRepository { get; }
    public IUserRoleRepository UserRoleRepository { get; }
    public IRoleRepository RoleRepository { get; }

    Task SaveAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}