using Imprink.Application.Users.Dtos;
using Imprink.Domain.Entities.Users;
using MediatR;

namespace Imprink.Application.Users;

public record DeleteUserRoleCommand(string Sub, Guid RoleId) : IRequest<UserRoleDto?>;

public class DeleteUserRoleHandler(IUnitOfWork uw) : IRequestHandler<DeleteUserRoleCommand, UserRoleDto?>
{
    public async Task<UserRoleDto?> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken)) return null;

        var userRole = new UserRole
        {
            UserId = request.Sub,
            RoleId = request.RoleId
        };
        
        var removedRole = await uw.UserRoleRepository.RemoveUserRoleAsync(userRole, cancellationToken);

        return new UserRoleDto
        {
            UserId = removedRole.UserId,
            RoleId = removedRole.RoleId
        };
    }
}