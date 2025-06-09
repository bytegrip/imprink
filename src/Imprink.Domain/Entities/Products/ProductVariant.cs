using Imprink.Domain.Entities.Orders;

namespace Imprink.Domain.Entities.Product;

public class ProductVariant : EntityBase
{
    public required Guid ProductId { get; set; }
    public required string Size { get; set; }
    public string? Color { get; set; }
    public required decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public required string Sku { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
        
    public virtual required Imprink.Domain.Entities.Product.Product Product { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}