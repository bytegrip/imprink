namespace Printbase.Domain.Entities.Orders;

public class OrderAddress : EntityBase
{
    public Guid OrderId { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
        
    public virtual Order Order { get; set; }
}