using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductGroups")]
public class ProductGroupDbEntity
{
    [Key, Required]
    public Guid Id { get; set; }
    
    [MaxLength(50), Required]
    public required string Name { get; set; }
    
    [MaxLength(255)]
    public string? Description { get; set; }
    
    [InverseProperty(nameof(ProductTypeDbEntity.Group)), Required]
    public required ICollection<ProductTypeDbEntity> Types { get; set; }
}