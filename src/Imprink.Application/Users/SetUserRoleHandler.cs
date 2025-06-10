using Imprink.Application.Exceptions;
using Imprink.Application.Users.Dtos;
using Imprink.Domain.Entities.Users;
using MediatR;

namespace Imprink.Application.Users;

public record SetUserRoleCommand(string Sub, Guid RoleId) : IRequest<UserRoleDto?>;

public class SetUserRoleHandler(IUnitOfWork uw) : IRequestHandler<SetUserRoleCommand, UserRoleDto?>
{
    public async Task<UserRoleDto?> Handle(SetUserRoleCommand request, CancellationToken cancellationToken)
    {
        if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken))
            throw new NotFoundException("User with ID: " + request.Sub + " does not exist.");

        var userRole = new UserRole
        {
            UserId = request.Sub,
            RoleId = request.RoleId
        };
        
        var addedRole = await uw.UserRoleRepository.AddUserRoleAsync(userRole, cancellationToken);

        return new UserRoleDto
        {
            UserId = addedRole.UserId,
            RoleId = addedRole.RoleId
        };
    }
}