using Imprink.Application;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;

namespace Imprink.Infrastructure;

public class UnitOfWork(
    ApplicationDbContext context, 
    IProductRepository productRepository, 
    IProductVariantRepository productVariantRepository, 
    ICategoryRepository categoryRepository,
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IRoleRepository roleRepository,
    IOrderRepository orderRepository,
    IOrderItemRepository orderItemRepository) : IUnitOfWork
{
    public IProductRepository ProductRepository => productRepository;
    public IProductVariantRepository ProductVariantRepository => productVariantRepository;
    public ICategoryRepository CategoryRepository => categoryRepository;
    public IUserRepository UserRepository => userRepository;
    public IUserRoleRepository UserRoleRepository => userRoleRepository;
    public IRoleRepository RoleRepository => roleRepository;
    public IOrderRepository OrderRepository => orderRepository;
    public IOrderItemRepository OrderItemRepository => orderItemRepository;

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.RollbackTransactionAsync(cancellationToken);
    }
    
    public async Task<T> TransactAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await operation();
            await SaveAsync(cancellationToken);
            await CommitTransactionAsync(cancellationToken);
            return result;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
    
    public async Task TransactAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        await BeginTransactionAsync(cancellationToken);
        try
        {
            await operation();
            await SaveAsync(cancellationToken);
            await CommitTransactionAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}