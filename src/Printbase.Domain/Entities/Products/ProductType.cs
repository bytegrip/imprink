namespace Printbase.Domain.Entities.Products;

public class ProductType
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid GroupId { get; set; }
    public ProductGroup Group { get; set; } = null!;
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}