namespace Printbase.Domain.Entities;

public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomizationImageUrl { get; set; }
    public string CustomizationDescription { get; set; }
        
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
    public virtual ProductVariant ProductVariant { get; set; }
}