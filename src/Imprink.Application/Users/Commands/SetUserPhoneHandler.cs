using AutoMapper;
using Imprink.Application.Exceptions;
using Imprink.Application.Services;
using Imprink.Application.Users.Dtos;
using MediatR;

namespace Imprink.Application.Domains.Users;

public record SetUserPhoneCommand(string PhoneNumber) : IRequest<UserDto?>;

public class SetUserPhoneHandler(IUnitOfWork uw, IMapper mapper, ICurrentUserService userService) : IRequestHandler<SetUserPhoneCommand, UserDto?>
{
    public async Task<UserDto?> Handle(SetUserPhoneCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            var currentUser = userService.GetCurrentUserId();
            
            if (currentUser == null)
                throw new NotFoundException("User token could not be accessed.");

            var user = await uw.UserRepository.SetUserPhoneAsync(currentUser, request.PhoneNumber, cancellationToken);
            
            if (user == null)
                throw new DataUpdateException("User phone could not be updated.");

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