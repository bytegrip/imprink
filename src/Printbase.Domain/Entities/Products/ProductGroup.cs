namespace Printbase.Domain.Entities.Products;

public class ProductGroup
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<ProductType> Types { get; set; } = [];
}