using Imprink.Domain.Entities.Users;

namespace Imprink.Domain.Repositories;

public interface IUserRepository
{
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
    Task UpdateLastLoginAsync(string userId, DateTime loginTime, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    
    Task<(IEnumerable<User> Users, int TotalCount)> GetUsersPagedAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm = null,
        bool? isActive = null,
        CancellationToken cancellationToken = default);
    
    Task<User?> GetUserWithAddressesAsync(string userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<User?> GetUserWithAllRelatedDataAsync(string userId, CancellationToken cancellationToken = default);
}