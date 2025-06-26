using AutoMapper;
using Imprink.Application.Dtos;
using MediatR;

namespace Imprink.Application.Commands.Users;

public record GetAllRolesCommand : IRequest<IEnumerable<RoleDto>>;

public class GetAllRoles(
    IUnitOfWork uw, 
    IMapper mapper): IRequestHandler<GetAllRolesCommand, IEnumerable<RoleDto>>
{
    public async Task<IEnumerable<RoleDto>> Handle(
        GetAllRolesCommand request, 
        CancellationToken cancellationToken)
    {
        var roles = await uw.RoleRepository
            .GetAllRolesAsync(cancellationToken);

        return mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}