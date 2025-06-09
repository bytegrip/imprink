using Imprink.Domain.Entities.Users;

namespace Imprink.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IEnumerable<Role>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersInRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    Task<bool> IsUserInRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default);
    
    Task<UserRole?> GetUserRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default);
    Task AddUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task RemoveUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}