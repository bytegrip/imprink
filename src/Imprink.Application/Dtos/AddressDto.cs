namespace Imprink.Application.Dtos;

public class AddressDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public string AddressType { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Company { get; set; }
    public string AddressLine1 { get; set; } = null!;
    public string? AddressLine2 { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? BuildingNumber { get; set; }
    public string? Floor { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Instructions { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
}