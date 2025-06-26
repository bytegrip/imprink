namespace Imprink.Domain.Entities;

public class Product : EntityBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required decimal BasePrice { get; set; }
    public required bool IsCustomizable { get; set; }
    public required bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
        
    public virtual required Category Category { get; set; }
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}