using Imprink.Domain.Entities.Users;
using Imprink.Domain.Repositories;
using MediatR;

namespace Imprink.Application.Users.Roles;

public record AddUserToRoleCommand(string UserId, Guid RoleId) : IRequest<bool>;

public class AddUserToRoleCommandHandler(
    IUserRoleRepository userRoleRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository)
    : IRequestHandler<AddUserToRoleCommand, bool>
{
    public async Task<bool> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.UserExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            return false;

        var roleExists = await roleRepository.RoleExistsAsync(request.RoleId, cancellationToken);
        if (!roleExists)
            return false;

        var isAlreadyInRole = await userRoleRepository.IsUserInRoleAsync(request.UserId, request.RoleId, cancellationToken);
        if (isAlreadyInRole)
            return true; 

        var userRole = new UserRole
        {
            UserId = request.UserId,
            RoleId = request.RoleId
        };

        await userRoleRepository.AddUserRoleAsync(userRole, cancellationToken);
        
        return true;
    }
}