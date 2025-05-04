using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductVariants")]
[Index(nameof(ProductId), nameof(Color), nameof(Size), IsUnique = true)]
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
    [Range(0.01, 9999999.99)]
    public decimal Price { get; set; }
    
    [Range(0, 100)]
    public decimal? Discount { get; set; }
    
    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
    
    [MaxLength(50)]
    public string? SKU { get; set; }
    
    [ForeignKey(nameof(ProductId)), Required]
    public required ProductDbEntity Product { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
}