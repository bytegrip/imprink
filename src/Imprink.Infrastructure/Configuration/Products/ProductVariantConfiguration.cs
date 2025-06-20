using Imprink.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Products;

public class ProductVariantConfiguration : EntityBaseConfiguration<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);
            
        builder.Property(pv => pv.ProductId)
            .IsRequired();
                
        builder.Property(pv => pv.Size)
            .HasMaxLength(50);
                
        builder.Property(pv => pv.Color)
            .HasMaxLength(50);
                
        builder.Property(pv => pv.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
                
        builder.Property(pv => pv.ImageUrl)
            .HasMaxLength(500);
                
        builder.Property(pv => pv.Sku)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(pv => pv.StockQuantity)
            .IsRequired()
            .HasDefaultValue(0);
                
        builder.Property(pv => pv.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(c => c.CreatedAt)
            .IsRequired(false);
        
        builder.Property(c => c.CreatedBy)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedAt)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedBy)
            .IsRequired(false);

        builder.HasOne(pv => pv.Product)
            .WithMany(p => p.ProductVariants)
            .HasForeignKey(pv => pv.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pv => pv.ProductId)
            .HasDatabaseName("IX_ProductVariant_ProductId");
                
        builder.HasIndex(pv => pv.Sku)
            .IsUnique()
            .HasDatabaseName("IX_ProductVariant_SKU");
                
        builder.HasIndex(pv => pv.IsActive)
            .HasDatabaseName("IX_ProductVariant_IsActive");
                
        builder.HasIndex(pv => new { pv.ProductId, pv.Size, pv.Color })
            .IsUnique()
            .HasDatabaseName("IX_ProductVariant_Product_Size_Color");
    }
}