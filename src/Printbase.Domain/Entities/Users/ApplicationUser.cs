using Microsoft.AspNetCore.Identity;

namespace Printbase.Domain.Entities.Users;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public string ProfileImageUrl { get; set; }
        
    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}