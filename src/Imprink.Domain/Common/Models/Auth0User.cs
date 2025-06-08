namespace Imprink.Domain.Common.Models;

public class Auth0User
{
    public string Sub { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailVerified { get; set; }
}