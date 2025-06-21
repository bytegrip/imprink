using Imprink.Domain.Entities;

namespace Imprink.Domain.Repositories;

public interface IUserRoleRepository
{
    Task<IEnumerable<Role>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetUsersInRoleAsync(Guid roleId, CancellationToken cancellationToken = default);
    
    Task<UserRole?> GetUserRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default);
    Task<UserRole> AddUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default);
    Task<UserRole> RemoveUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default);
}