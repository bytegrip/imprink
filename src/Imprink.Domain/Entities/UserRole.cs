namespace Imprink.Domain.Entities;

public class UserRole
{
    public string UserId { get; set; } = null!;
    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}