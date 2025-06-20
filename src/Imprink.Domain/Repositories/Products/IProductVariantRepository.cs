using Imprink.Domain.Entities.Products;

namespace Imprink.Domain.Repositories.Products;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductVariant?> GetByIdWithProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetActiveByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProductVariant>> GetInStockByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<ProductVariant> AddAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
    Task<ProductVariant> UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task UpdateStockQuantityAsync(Guid id, int quantity, CancellationToken cancellationToken = default);
}