using Imprink.Domain.Entities;

namespace Imprink.Domain.Repositories;

public interface IOrderAddressRepository
{
    Task<OrderAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrderAddress?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderAddress>> GetByOrderIdsAsync(IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default);
    Task<OrderAddress> AddAsync(OrderAddress orderAddress, CancellationToken cancellationToken = default);
    Task<OrderAddress> UpdateAsync(OrderAddress orderAddress, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
}