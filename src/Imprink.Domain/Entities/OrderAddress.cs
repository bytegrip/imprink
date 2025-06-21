namespace Imprink.Domain.Entities;

public class OrderAddress : EntityBase
{
    public Guid OrderId { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
        
    public virtual required Order Order { get; set; }
}