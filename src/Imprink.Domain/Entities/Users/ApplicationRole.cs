using Microsoft.AspNetCore.Identity;

namespace Imprink.Domain.Entities.Users;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
        
    public ApplicationRole()
    {}
        
    public ApplicationRole(string roleName) : base(roleName)
    {}
}