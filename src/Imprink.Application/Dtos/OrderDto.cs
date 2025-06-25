namespace Imprink.Application.Dtos;

public class OrderDto
{
    public Guid Id { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public OrderStatusDto? OrderStatus { get; set; }
    public UserDto? User { get; set; }
    public ShippingStatusDto? ShippingStatus { get; set; }
    public OrderAddressDto? OrderAddress { get; set; }
    public ProductDto? Product { get; set; }
    public ProductVariantDto? ProductVariant { get; set; }
}