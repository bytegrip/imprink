using Imprink.Application.Exceptions;
using Imprink.Application.Users.Dtos;
using Imprink.Domain.Entities.Users;
using MediatR;

namespace Imprink.Application.Users;

public record DeleteUserRoleCommand(string Sub, Guid RoleId) : IRequest<UserRoleDto?>;

public class DeleteUserRoleHandler(IUnitOfWork uw) : IRequestHandler<DeleteUserRoleCommand, UserRoleDto?>
{
    public async Task<UserRoleDto?> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken))
                throw new NotFoundException("User with ID: " + request.Sub + " does not exist.");

            var userRole = new UserRole
            {
                UserId = request.Sub,
                RoleId = request.RoleId
            };

            var removedRole = await uw.UserRoleRepository.RemoveUserRoleAsync(userRole, cancellationToken);

            await uw.SaveAsync(cancellationToken);
            await uw.CommitTransactionAsync(cancellationToken);
            
            return new UserRoleDto
            {
                UserId = removedRole.UserId,
                RoleId = removedRole.RoleId
            };
        }
        catch
        {
            await uw.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        
    }
}