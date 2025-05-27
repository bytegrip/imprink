namespace Printbase.Domain.Entities.Orders;

public class Order : EntityBase
{
    public string UserId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public int OrderStatusId { get; set; }
    public int ShippingStatusId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string Notes { get; set; } = null!;
        
    public OrderStatus OrderStatus { get; set; } = null!;
    public ShippingStatus ShippingStatus { get; set; } = null!;
    public OrderAddress OrderAddress { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}