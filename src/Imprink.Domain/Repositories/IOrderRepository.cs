using Imprink.Domain.Entities;
using Imprink.Domain.Models;

namespace Imprink.Domain.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByUserIdWithDetailsAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByMerchantIdAsync(string merchantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByMerchantIdWithDetailsAsync(string merchantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByStatusAsync(int statusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByShippingStatusAsync(int shippingStatusId, CancellationToken cancellationToken = default);
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid orderId, int statusId, CancellationToken cancellationToken = default);
    Task UpdateShippingStatusAsync(Guid orderId, int shippingStatusId, CancellationToken cancellationToken = default);
    Task AssignMerchantAsync(Guid orderId, string merchantId, CancellationToken cancellationToken = default);
    Task UnassignMerchantAsync(Guid orderId, CancellationToken cancellationToken = default);
}