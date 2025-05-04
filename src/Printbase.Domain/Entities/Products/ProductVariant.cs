namespace Printbase.Domain.Entities.Products;

public class ProductVariant
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public int Stock { get; set; }
    public string? SKU { get; set; }
    public Product Product { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    
    public decimal GetDiscountedPrice()
    {
        if (Discount is > 0)
        {
            return Price - Price * Discount.Value / 100m;
        }
        
        return Price;
    }
}