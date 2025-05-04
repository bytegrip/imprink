using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("Products")]
public class ProductDbEntity
{
    [Key, Required]
    public Guid Id { get; set; }
    
    [MaxLength(50), Required]
    public required string Name { get; set; }
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public Guid TypeId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
    
    [ForeignKey(nameof(TypeId)), Required]
    public required ProductTypeDbEntity Type { get; set; }

    [InverseProperty(nameof(ProductVariantDbEntity.Product)), Required]
    public required ICollection<ProductVariantDbEntity> Variants { get; set; } = [];
}