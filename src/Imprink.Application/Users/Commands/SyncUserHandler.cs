using AutoMapper;
using Imprink.Application.Users.Dtos;
using Imprink.Domain.Models;
using MediatR;

namespace Imprink.Application.Users.Commands;

public record SyncUserCommand(Auth0User User) : IRequest<UserDto?>;

public class SyncUserHandler(IUnitOfWork uw, IMapper mapper): IRequestHandler<SyncUserCommand, UserDto?>
{
    public async Task<UserDto?> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            var user = await uw.UserRepository.UpdateOrCreateUserAsync(request.User, cancellationToken);
            
            if (user == null) 
                throw new Exception("User exists but could not be updated");
            
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