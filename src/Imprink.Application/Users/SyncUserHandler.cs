using Imprink.Domain.Common.Models;
using MediatR;

namespace Imprink.Application.Users;

public record SyncUserCommand(Auth0User User) : IRequest<bool>;

public class SyncUserHandler(IUnitOfWork uw): IRequestHandler<SyncUserCommand, bool>
{
    public async Task<bool> Handle(SyncUserCommand request, CancellationToken cancellationToken)
    {
        await uw.BeginTransactionAsync(cancellationToken);

        try
        {
            if (!await uw.UserRepository.UpdateOrCreateUserAsync(request.User, cancellationToken))
            {
                await uw.RollbackTransactionAsync(cancellationToken);
            }

            await uw.SaveAsync(cancellationToken);
            await uw.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch
        {
            await uw.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}