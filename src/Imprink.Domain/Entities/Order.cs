namespace Imprink.Domain.Entities;

public class Order : EntityBase
{
    public string UserId { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
    public int Quantity { get; set; }
    public Guid ProductId { get; set; }
    public Guid? ProductVariantId { get; set; }
    public int OrderStatusId { get; set; }
    public int ShippingStatusId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string? Notes { get; set; }
    public string? MerchantId { get; set; }
    public string? ComposingImageUrl { get; set; }
    public string[] OriginalImageUrls { get; set; } = [];
    public string CustomizationImageUrl { get; set; } = null!;
    public string CustomizationDescription { get; set; } = null!;
        
    public OrderStatus OrderStatus { get; set; } = null!;
    public User User { get; set; } = null!;
    public ShippingStatus ShippingStatus { get; set; } = null!;
    public OrderAddress OrderAddress { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public ProductVariant? ProductVariant { get; set; }
}