using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using Imprink.Application.Services;
using MediatR;

namespace Imprink.Application.Commands.Users;

public record SetUserFullNameCommand(string FirstName, string LastName) : IRequest<UserDto?>;

public class SetUserFullName(
    IUnitOfWork uw, 
    IMapper mapper, 
    ICurrentUserService userService) 
    : IRequestHandler<SetUserFullNameCommand, UserDto?>
{
    public async Task<UserDto?> Handle(
        SetUserFullNameCommand request, 
        CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            var currentUser = userService.GetCurrentUserId();
            
            if (currentUser == null)
                throw new NotFoundException("User token could not be accessed.");

            var user = await uw.UserRepository
                .SetUserFullNameAsync(currentUser, request.FirstName, request.LastName, cancellationToken);
            
            if (user == null)
                throw new DataUpdateException("User name could not be updated.");

            await uw.SaveAsync(cancellationToken);
            await uw.CommitTransactionAsync(cancellationToken);

            return mapper.Map<UserDto>(user);
        }
        catch
        {
            await uw.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    } 
}