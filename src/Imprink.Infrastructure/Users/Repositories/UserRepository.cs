using Imprink.Domain.Entities.Users;
using Imprink.Domain.Models;
using Imprink.Domain.Repositories.Users;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Users.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> UpdateOrCreateUserAsync(Auth0User user, CancellationToken cancellationToken = default)
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
            return newUser;
        }

        userToUpdate.Email = user.Email;
        userToUpdate.Name = user.Name;
        userToUpdate.Nickname = user.Nickname;
        userToUpdate.EmailVerified = user.EmailVerified;

        return userToUpdate;
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

    public async Task<bool> UserExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(u => u.Id == userId, cancellationToken);
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

    public async Task<User?> SetUserPhoneAsync(string userId, string phoneNumber, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    
        if (user == null) return null;
    
        user.PhoneNumber = phoneNumber;
        return user;
    }

    public async Task<User?> SetUserFullNameAsync(string userId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    
        if (user == null) return null;

        user.FirstName = firstName;
        user.LastName = lastName;
        return user;
    }
}