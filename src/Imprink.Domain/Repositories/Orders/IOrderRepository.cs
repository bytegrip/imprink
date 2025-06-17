using Imprink.Domain.Entities.Orders;
using Imprink.Domain.Models;

namespace Imprink.Domain.Repositories.Orders;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdWithAddressAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<PagedResult<Order>> GetPagedAsync(OrderFilterParameters filterParameters, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByUserIdPagedAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByOrderStatusAsync(int orderStatusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByShippingStatusAsync(int shippingStatusId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> OrderNumberExistsAsync(string orderNumber, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default);
    Task<decimal> GetTotalRevenueByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<int> GetOrderCountByStatusAsync(int orderStatusId, CancellationToken cancellationToken = default);
}