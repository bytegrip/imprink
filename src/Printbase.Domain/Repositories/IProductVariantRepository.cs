using Printbase.Domain.Entities.Products;

namespace Printbase.Domain.Repositories;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductVariant> AddAsync(ProductVariant variant, CancellationToken cancellationToken = default);
    Task<ProductVariant> UpdateAsync(ProductVariant variant, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}