namespace Printbase.Domain.Entities.Orders;

public class ShippingStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
        
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}