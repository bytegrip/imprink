using Imprink.Domain.Entities.Orders;

namespace Imprink.Domain.Entities.Users;

public class User
{
    public string Id { get; set; } = null!;
    public required string Name { get; set; }
    public required string Nickname { get; set; }
    public required string Email { get; set; }
    public bool EmailVerified { get; set; }
    
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public required bool IsActive { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public Address? DefaultAddress => Addresses.FirstOrDefault(a => a is { IsDefault: true, IsActive: true });
    public IEnumerable<Role> Roles => UserRoles.Select(ur => ur.Role);
}