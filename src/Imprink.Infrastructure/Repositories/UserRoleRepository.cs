using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class UserRoleRepository(ApplicationDbContext context) : IUserRoleRepository
{
    public async Task<IEnumerable<Role>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserRole
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersInRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.UserRole
            .AsNoTracking()
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<UserRole?> GetUserRoleAsync(string userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.UserRole
            .AsNoTracking()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);
    }

    public Task<UserRole> AddUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        var ur = context.UserRole.Add(userRole);
        return Task.FromResult(ur.Entity);
    }

    public Task<UserRole> RemoveUserRoleAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        var ur = context.UserRole.Remove(userRole);
        return Task.FromResult(ur.Entity);
    }
}