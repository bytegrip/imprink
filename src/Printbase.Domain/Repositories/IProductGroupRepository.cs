using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductGroupRepository
{
    Task<ProductGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductGroup>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ProductGroup productGroup, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductGroup productGroup, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}