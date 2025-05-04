using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductGroupRepository
{
    Task<ProductGroup?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductGroup>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<ProductGroup> AddAsync(ProductGroup group, CancellationToken cancellationToken = default);
    Task<ProductGroup> UpdateAsync(ProductGroup group, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}