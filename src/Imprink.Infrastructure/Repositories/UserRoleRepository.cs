using Imprink.Domain.Entities.Users;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class UserRoleRepository(ApplicationDbContext context) : IUserRoleRepository
{
    public async Task<IEnumerable<Role>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersInRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUserInRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);
    }

    public async Task<UserRole?> GetUserRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);
    }

    public async Task AddUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        context.UserRoles.Add(userRole);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        context.UserRoles.Remove(userRole);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserRole>> GetUserRolesByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .ToListAsync(cancellationToken);
    }
}