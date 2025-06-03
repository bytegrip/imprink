namespace Imprink.Domain.Entities.Users;

public class Address : EntityBase
{
    public required string UserId { get; set; }
    public required string AddressType { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
    public required bool IsDefault { get; set; }
    public required bool IsActive { get; set; }
}