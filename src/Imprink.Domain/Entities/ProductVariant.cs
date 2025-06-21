namespace Imprink.Domain.Entities;

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
        
    public virtual required Product Product { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}