namespace Imprink.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string RoleName { get; set; } = null!;
        
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}