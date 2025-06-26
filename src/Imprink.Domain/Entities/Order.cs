namespace Imprink.Domain.Entities;

public class Order : EntityBase
{
    public required string UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public int OrderStatusId { get; set; }
    public int ShippingStatusId { get; set; }
    public string? Notes { get; set; }
    public string? MerchantId { get; set; }
    public string? CustomizationImageUrl { get; set; }
    public string[] OriginalImageUrls { get; set; } = [];
    public string? CustomizationDescription { get; set; }
        
    public virtual OrderStatus OrderStatus { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual User? Merchant { get; set; }
    public virtual ShippingStatus ShippingStatus { get; set; } = null!;
    public virtual OrderAddress OrderAddress { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual ProductVariant? ProductVariant { get; set; }
}