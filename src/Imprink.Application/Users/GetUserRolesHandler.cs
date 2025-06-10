using Imprink.Domain.Entities.Users;
using MediatR;

namespace Imprink.Application.Users;

public record GetUserRolesCommand(string Sub) : IRequest<IEnumerable<Role>>;

public class GetUserRolesHandler(IUnitOfWork uw): IRequestHandler<GetUserRolesCommand, IEnumerable<Role>>
{
    public async Task<IEnumerable<Role>> Handle(GetUserRolesCommand request, CancellationToken cancellationToken)
    {
        if (await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken)) return [];
        return await uw.UserRoleRepository.GetUserRolesAsync(request.Sub, cancellationToken);;
    }
}