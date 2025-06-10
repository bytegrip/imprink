using Imprink.Application.Exceptions;
using Imprink.Application.Service;
using Imprink.Application.Users.Dtos;
using MediatR;

namespace Imprink.Application.Users;

public record SetUserPhoneCommand(string PhoneNumber) : IRequest<UserDto?>;

public class SetUserPhoneHandler(IUnitOfWork uw, ICurrentUserService userService) : IRequestHandler<SetUserPhoneCommand, UserDto?>
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

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Nickname = user.Nickname,
                Email = user.Email,
                EmailVerified = user.EmailVerified,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive
            };
        }
        catch
        {
            await uw.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    } 
}