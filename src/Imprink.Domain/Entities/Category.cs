namespace Imprink.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = null!; 
    public string Description { get; set; } = null!; 
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
    
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}