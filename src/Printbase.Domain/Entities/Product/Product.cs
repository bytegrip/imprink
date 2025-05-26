using Printbase.Domain.Entities.Orders;

namespace Printbase.Domain.Entities.Product;

public class Product : EntityBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsCustomizable { get; set; }
    public bool IsActive { get; set; }
    public string ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
        
    public virtual Category Category { get; set; }
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}