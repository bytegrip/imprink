using Imprink.Domain.Entities.Users;
using Imprink.Domain.Repositories.Users;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Users.Repositories;

public class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
    public async Task<IEnumerable<Role>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await context.Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        return await context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RoleName == roleName, cancellationToken);
    }

    public async Task<bool> RoleExistsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.Roles
            .AnyAsync(r => r.Id == roleId, cancellationToken);
    }
}