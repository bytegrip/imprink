using Imprink.Application.Exceptions;
using Imprink.Application.Service;
using Imprink.Application.Users.Dtos;
using MediatR;

namespace Imprink.Application.Users;

public record SetUserFullNameCommand(string FirstName, string LastName) : IRequest<UserDto?>;

public class SetUserFullNameHandler(IUnitOfWork uw, ICurrentUserService userService) : IRequestHandler<SetUserFullNameCommand, UserDto?>
{
    public async Task<UserDto?> Handle(SetUserFullNameCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            var currentUser = userService.GetCurrentUserId();
            if (currentUser == null)
                throw new NotFoundException("User token could not be accessed.");

            var user = await uw.UserRepository.SetUserFullNameAsync(currentUser, request.FirstName, request.LastName, cancellationToken);
            if (user == null)
                throw new DataUpdateException("User name could not be updated.");

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