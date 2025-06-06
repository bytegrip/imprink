using Imprink.Domain.Entities.Orders;

namespace Imprink.Domain.Entities.Users;

public class User : EntityBase
{
    public new string Id { get; set; } = null!;
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public required bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public string FullName => $"{FirstName} {LastName}";
    public Address? DefaultAddress => Addresses.FirstOrDefault(a => a.IsDefault && a.IsActive);
    public IEnumerable<Role> Roles => UserRoles.Select(ur => ur.Role);
}