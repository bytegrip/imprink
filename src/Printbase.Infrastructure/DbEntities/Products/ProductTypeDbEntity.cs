using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductTypes")]
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
    
    [InverseProperty(nameof(ProductDbEntity.Type))]
    public ICollection<ProductDbEntity>? Products { get; set; }
}