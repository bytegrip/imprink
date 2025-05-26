using Printbase.Domain.Entities.Product;

namespace Printbase.Domain.Entities.Orders;

public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomizationImageUrl { get; set; } = default!;
    public string CustomizationDescription { get; set; } = default!;
        
    public virtual Order Order { get; set; } = default!;
    public virtual Product.Product Product { get; set; } = default!;
    public virtual ProductVariant ProductVariant { get; set; } = default!;
}