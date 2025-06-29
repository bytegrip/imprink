using AutoMapper;
using Imprink.Application.Dtos;
using Imprink.Application.Exceptions;
using MediatR;

namespace Imprink.Application.Commands.Users;

public record GetUserRolesCommand(string Sub) : IRequest<IEnumerable<RoleDto>>;

public class GetUserRoles(
    IUnitOfWork uw, 
    IMapper mapper)
    : IRequestHandler<GetUserRolesCommand, IEnumerable<RoleDto>>
{
    public async Task<IEnumerable<RoleDto>> Handle(
        GetUserRolesCommand request, 
        CancellationToken cancellationToken)
    {
        if (!await uw.UserRepository.UserExistsAsync(request.Sub, cancellationToken)) 
            throw new NotFoundException("User with ID: " + request.Sub + " does not exist.");
        
        var roles = await uw.UserRoleRepository
            .GetUserRolesAsync(request.Sub, cancellationToken);

        return mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}