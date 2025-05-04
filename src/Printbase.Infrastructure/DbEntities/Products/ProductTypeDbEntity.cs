using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductTypes")]
[Index(nameof(Name), nameof(GroupId), IsUnique = true)]
public class ProductTypeDbEntity
{
    [Key, Required]
    public Guid Id { get; set; }
    
    [MaxLength(50), Required]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [Required]
    public Guid GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId)), Required]
    public required ProductGroupDbEntity Group { get; set; }
    
    [InverseProperty(nameof(ProductDbEntity.Type)), Required]
    public required ICollection<ProductDbEntity> Products { get; set; } = [];
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required]
    public bool IsActive { get; set; } = true;
}