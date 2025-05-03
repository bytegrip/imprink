using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductVariants")]
public class ProductVariantDbEntity
{
    [Key, Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [MaxLength(50)]
    public string? Color { get; set; }
    
    [MaxLength(20)]
    public string? Size { get; set; }
    
    [Column(TypeName = "decimal(18,2)"), Required]
    public decimal Price { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal? Discount { get; set; }
    
    [Required]
    public int Stock { get; set; }
    
    [ForeignKey(nameof(ProductId)), Required]
    public required ProductDbEntity Product { get; set; }
}