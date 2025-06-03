using Imprink.Domain.Entities.Orders;

namespace Imprink.Domain.Entities.Product;

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
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}