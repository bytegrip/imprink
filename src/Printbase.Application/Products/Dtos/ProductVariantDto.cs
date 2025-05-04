namespace Printbase.Application.Products.Dtos;

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public string? Color { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public decimal DiscountedPrice => Discount is > 0 ? Price - Price * Discount.Value / 100m : Price;
    public int Stock { get; set; }
    public string? SKU { get; set; }
    public bool IsActive { get; set; }
}