namespace Imprink.Application.Users.Dtos;

public class UserRoleDto
{
    public required string UserId { get; set; }
    public required Guid RoleId { get; set; }
}