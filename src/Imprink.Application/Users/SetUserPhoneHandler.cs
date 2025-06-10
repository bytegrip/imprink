using Imprink.Application.Users.Dtos;
using MediatR;

namespace Imprink.Application.Users;

public record SetUserPhoneCommand(string Sub, Guid RoleId) : IRequest<UserRoleDto?>;

public class SetUserPhoneHandler
{
    
}