using Imprink.Domain.Entities.Users;
using Imprink.Domain.Models;

namespace Imprink.Domain.Repositories.Users;

public interface IUserRepository
{
    Task<User?> UpdateOrCreateUserAsync(Auth0User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<User?> SetUserPhoneAsync(string userId, string phoneNumber, CancellationToken cancellationToken = default);
    Task<User?> SetUserFullNameAsync(string userId, string firstName, string lastName, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithAllRelatedDataAsync(string userId, CancellationToken cancellationToken = default);
}