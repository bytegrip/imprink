using Imprink.Domain.Entities;

namespace Imprink.Domain.Repositories;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderItem?> GetByIdWithProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderItem?> GetByIdWithVariantAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderItem?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByOrderIdWithProductsAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(Guid productVariantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetCustomizedItemsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<OrderItem> AddAsync(OrderItem orderItem, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> AddRangeAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken = default);
    Task<OrderItem> UpdateAsync(OrderItem orderItem, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalValueByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<int> GetQuantityByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<int> GetQuantityByVariantIdAsync(Guid productVariantId, CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, int>> GetProductSalesCountAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);
}