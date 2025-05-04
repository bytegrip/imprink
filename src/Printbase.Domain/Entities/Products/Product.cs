namespace Printbase.Domain.Entities.Products;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid TypeId { get; init; }
    public ProductType Type { get; init; } = null!;
    public ICollection<ProductVariant> Variants { get; init; } = [];
}