using Imprink.Domain.Common.Models;
using Imprink.Domain.Entities.Users;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<bool> UpdateOrCreateUserAsync(Auth0User user, CancellationToken cancellationToken = default)
    {
        var userToUpdate = await context.Users
            .Where(u => u.Id.Equals(user.Sub))
            .FirstOrDefaultAsync(cancellationToken);
        
        if (userToUpdate == null)
        {
            var newUser = new User
            {
                Id = user.Sub,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                Name = user.Name,
                Nickname = user.Nickname,
                IsActive = true
            };
            
            context.Users.Add(newUser);
        }
        else
        {
            userToUpdate.Email = user.Email;
            userToUpdate.Name = user.Name;
            userToUpdate.Nickname = user.Nickname;
            userToUpdate.EmailVerified = user.EmailVerified;
        }
        
        return true;
    }

    public async Task<User?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ActivateUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null) return false;

        user.IsActive = true;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeactivateUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync(new object[] { userId }, cancellationToken);
        if (user == null) return false;

        user.IsActive = false;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserWithAllRelatedDataAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.Addresses)
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.Orders)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }
}