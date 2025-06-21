namespace Imprink.Domain.Entities;

public class OrderItem : EntityBase
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string CustomizationImageUrl { get; set; } = null!;
    public string CustomizationDescription { get; set; } = null!;
        
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductVariant ProductVariant { get; set; } = null!;
}