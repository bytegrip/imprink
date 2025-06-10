using Imprink.Application.Users.Dtos;
using Imprink.Domain.Models;
using MediatR;

namespace Imprink.Application.Users;

public record SyncUserCommand(Auth0User User) : IRequest<UserDto?>;

public class SyncUserHandler(IUnitOfWork uw): IRequestHandler<SyncUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = await uw.UserRepository.UpdateOrCreateUserAsync(request.User, cancellationToken);
            
            if (user == null) throw new Exception("User exists but could not be updated");
            
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