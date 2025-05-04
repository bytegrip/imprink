using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetByTypeIdAsync(Guid typeId, bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}