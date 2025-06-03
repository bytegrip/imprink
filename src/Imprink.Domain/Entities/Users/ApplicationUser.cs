using Imprink.Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Imprink.Domain.Entities.Users;

public sealed class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public string? ProfileImageUrl { get; set; }
        
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}