using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Printbase.Infrastructure.DbEntities.Products;

[Table("ProductTypes")]
public class ProductTypeDbEntity
{
    [Key, Required]
    public Guid Id { get; init; }
    
    [MaxLength(50), Required]
    public required string Name { get; init; }
    
    [MaxLength(255)]
    public string? Description { get; init; }
    
    [Required]
    public Guid GroupId { get; init; }
    
    [ForeignKey(nameof(GroupId)), Required]
    public required ProductGroupDbEntity Group { get; init; }
    
    [InverseProperty(nameof(ProductDbEntity.Type))]
    public ICollection<ProductDbEntity>? Products { get; init; }
}