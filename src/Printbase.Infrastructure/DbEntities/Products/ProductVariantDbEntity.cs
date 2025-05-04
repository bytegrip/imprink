using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductVariants")]
public class ProductVariantDbEntity
{
    [Key, Required]
    public Guid Id { get; init; }
    
    [Required]
    public Guid ProductId { get; init; }
    
    [MaxLength(50)]
    public string? Color { get; init; }
    
    [MaxLength(20)]
    public string? Size { get; init; }
    
    [Column(TypeName = "decimal(18,2)"), Required]
    public decimal Price { get; init; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Discount { get; init; }
    
    [Required]
    public int Stock { get; init; }
    
    [ForeignKey(nameof(ProductId)), Required]
    public required ProductDbEntity Product { get; init; }
}