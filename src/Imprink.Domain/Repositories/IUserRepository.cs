using Imprink.Domain.Common.Models;
using Imprink.Domain.Entities.Users;

namespace Imprink.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> UpdateOrCreateUserAsync(Auth0User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    
    Task<bool> ActivateUserAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> DeactivateUserAsync(string userId, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    
    Task<User?> GetUserWithAllRelatedDataAsync(string userId, CancellationToken cancellationToken = default);
}