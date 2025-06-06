using Imprink.Domain.Repositories;
using MediatR;

namespace Imprink.Application.Users.Roles;

public record RemoveUserFromRoleCommand(string UserId, Guid RoleId) : IRequest<bool>;

public class RemoveUserFromRoleCommandHandler(
    IUserRoleRepository userRoleRepository,
    IRoleRepository roleRepository,
    IUserRepository userRepository)
    : IRequestHandler<RemoveUserFromRoleCommand, bool>
{
    public async Task<bool> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
    {
        var userExists = await userRepository.UserExistsAsync(request.UserId, cancellationToken);
        if (!userExists)
            return false;

        var roleExists = await roleRepository.RoleExistsAsync(request.RoleId, cancellationToken);
        if (!roleExists)
            return false;

        var userRole = await userRoleRepository.GetUserRoleAsync(request.UserId, request.RoleId, cancellationToken);
        if (userRole == null)
            return true;

        await userRoleRepository.RemoveUserRoleAsync(userRole, cancellationToken);
        
        return true;
    }
}