namespace Printbase.Application.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid TypeId { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public ICollection<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}