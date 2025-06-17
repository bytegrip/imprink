using Imprink.Application.Exceptions;
using Imprink.Application.Users.Dtos;
using MediatR;

namespace Imprink.Application.Domains.Users;

public record GetUserRolesCommand(string Sub) : IRequest<IEnumerable<RoleDto>>;

public class GetUserRolesHandler(IUnitOfWork uw): IRequestHandler<GetUserRolesCommand, IEnumerable<RoleDto>>
{
    public async Task<IEnumerable<RoleDto>> Handle(GetUserRolesCommand request, CancellationToken cancellationToken)
    {
        if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken)) 
            throw new NotFoundException("User with ID: " + request.Sub + " does not exist.");
        
        var roles = await uw.UserRoleRepository.GetUserRolesAsync(request.Sub, cancellationToken);

        return roles.Select(role => new RoleDto
        {
            RoleId = role.Id,
            RoleName = role.RoleName
        }).ToList();
    }
}