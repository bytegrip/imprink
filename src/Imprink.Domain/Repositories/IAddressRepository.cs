using Imprink.Domain.Entities;

namespace Imprink.Domain.Repositories;

public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Address>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Address?> GetDefaultByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Address>> GetByUserIdAndTypeAsync(string userId, string addressType, CancellationToken cancellationToken = default);
    Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Address?> GetByIdAndUserIdAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task<Address> AddAsync(Address address, CancellationToken cancellationToken = default);
    Task<Address> UpdateAsync(Address address, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteByUserIdAsync(Guid id, string userId, CancellationToken cancellationToken = default);
    Task SetDefaultAddressAsync(string userId, Guid addressId, CancellationToken cancellationToken = default);
    Task DeactivateAddressAsync(Guid addressId, CancellationToken cancellationToken = default);
    Task ActivateAddressAsync(Guid addressId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsUserAddressAsync(Guid id, string userId, CancellationToken cancellationToken = default);
}