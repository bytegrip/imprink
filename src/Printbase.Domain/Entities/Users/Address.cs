namespace Printbase.Domain.Entities.Users;

public class Address : EntityBase
{
    public string UserId { get; set; }
    public string AddressType { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}