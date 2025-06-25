using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using MediatR;

namespace Imprink.Application.Commands.Users;

public record DeleteUserRoleCommand(string Sub, Guid RoleId) : IRequest<UserRoleDto?>;

public class DeleteUserRoleHandler(
    IUnitOfWork uw, 
    IMapper mapper) 
    : IRequestHandler<DeleteUserRoleCommand, UserRoleDto?>
{
    public async Task<UserRoleDto?> Handle(
        DeleteUserRoleCommand request, 
        CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken))
                throw new NotFoundException("User with ID: " + request.Sub + " does not exist.");

            var existingUserRole = await uw.UserRoleRepository
                .GetUserRoleAsync(request.Sub, request.RoleId, cancellationToken);
            
            if (existingUserRole == null)
                throw new NotFoundException($"User role not found for user {request.Sub} and role {request.RoleId}");

            var removedRole = await uw.UserRoleRepository
                .RemoveUserRoleAsync(existingUserRole, cancellationToken);

            await uw.SaveAsync(cancellationToken);
            await uw.CommitTransactionAsync(cancellationToken);
            
            return mapper.Map<UserRoleDto>(removedRole);
        }
        catch
        {
            await uw.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        
    }
}