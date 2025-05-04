using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("Products")]
public class ProductDbEntity
{
    [Key, Required]
    public Guid Id { get; init; }
    
    [MaxLength(50), Required]
    public required string Name { get; init; }
    
    [MaxLength(1000)]
    public string? Description { get; init; }
    
    [Required]
    public Guid TypeId { get; init; }
    
    [ForeignKey(nameof(TypeId)), Required]
    public required ProductTypeDbEntity Type { get; init; }

    [InverseProperty(nameof(ProductVariantDbEntity.Product)), Required]
    public required ICollection<ProductVariantDbEntity> Variants { get; init; } = [];
}