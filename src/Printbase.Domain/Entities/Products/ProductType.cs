namespace Printbase.Domain.Entities.Products;

public class ProductType
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid GroupId { get; init; }
    public ProductGroup Group { get; init; } = null!;
    public ICollection<Product>? Products { get; init; }
}