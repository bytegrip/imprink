using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductTypeRepository
{
    Task<ProductType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductType>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductType>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ProductType productType, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductType productType, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}