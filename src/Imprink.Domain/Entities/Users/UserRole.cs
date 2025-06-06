namespace Imprink.Domain.Entities.Users;

public class UserRole
{
    public string UserId { get; set; } = null!;
    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}