namespace Imprink.Domain.Entities;

public class Address : EntityBase
{
    public required string UserId { get; set; }
    public required string AddressType { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public required string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Instructions { get; set; }
    public required bool IsDefault { get; set; }
    public required bool IsActive { get; set; }
    
    public virtual User User { get; set; } = null!;
}