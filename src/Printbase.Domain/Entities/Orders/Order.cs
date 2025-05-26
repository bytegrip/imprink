namespace Printbase.Domain.Entities.Orders;

public class Order : EntityBase
{
    public string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int OrderStatusId { get; set; }
    public int ShippingStatusId { get; set; }
    public string OrderNumber { get; set; }
    public string Notes { get; set; }
        
    public virtual OrderStatus OrderStatus { get; set; }
    public virtual ShippingStatus ShippingStatus { get; set; }
    public virtual OrderAddress OrderAddress { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}