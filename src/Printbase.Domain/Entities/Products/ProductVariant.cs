namespace Printbase.Domain.Entities.Products;

public class ProductVariant
{
    public Guid Id { get; init; }
    public Guid ProductId { get; init; }
    public string? Color { get; init; }
    public string? Size { get; init; }
    public decimal Price { get; init; }
    public decimal? Discount { get; init; }
    public int Stock { get; init; }
    public Product Product { get; init; } = null!;
}