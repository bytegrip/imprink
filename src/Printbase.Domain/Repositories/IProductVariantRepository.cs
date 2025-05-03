using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}