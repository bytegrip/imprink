namespace Printbase.Domain.Entities;

public class ProductVariant : EntityBase
{
    public Guid ProductId { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public string SKU { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }
        
    public virtual Product Product { get; set; }
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}