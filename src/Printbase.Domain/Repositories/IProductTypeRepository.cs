using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductTypeRepository
{
    Task<ProductType?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductType>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductType>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<ProductType> AddAsync(ProductType type, CancellationToken cancellationToken = default);
    Task<ProductType> UpdateAsync(ProductType type, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}