using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductGroups")]
[Index(nameof(Name), IsUnique = true)]
public class ProductGroupDbEntity
{
    [Key, Required]
    public Guid Id { get; set; }
    
    [MaxLength(50), Required]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [InverseProperty(nameof(ProductTypeDbEntity.Group)), Required]
    public required ICollection<ProductTypeDbEntity> Types { get; set; } = [];
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
}