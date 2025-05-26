using Microsoft.AspNetCore.Identity;

namespace Printbase.Domain.Entities.Users;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
        
    public ApplicationRole() : base()
    {
    }
        
    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}