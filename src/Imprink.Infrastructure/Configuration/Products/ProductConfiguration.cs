using Imprink.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Products;

public class ProductConfiguration : EntityBaseConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);
            
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);
                
        builder.Property(p => p.Description)
            .HasMaxLength(2000);
                
        builder.Property(p => p.BasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
                
        builder.Property(p => p.IsCustomizable)
            .IsRequired()
            .HasDefaultValue(false);
                
        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
                
        builder.Property(p => p.ImageUrl)
            .HasMaxLength(500);

        builder.Property(p => p.CategoryId)
            .IsRequired(false);
        
        builder.Property(c => c.CreatedAt)
            .IsRequired(false);
        
        builder.Property(c => c.CreatedBy)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedAt)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedBy)
            .IsRequired(false);

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Product_Name");
                
        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Product_IsActive");
                
        builder.HasIndex(p => p.IsCustomizable)
            .HasDatabaseName("IX_Product_IsCustomizable");
            
        builder.HasIndex(p => p.CategoryId)
            .HasDatabaseName("IX_Product_CategoryId");
                
        builder.HasIndex(p => new { p.IsActive, p.IsCustomizable })
            .HasDatabaseName("IX_Product_Active_Customizable");
            
        builder.HasIndex(p => new { p.CategoryId, p.IsActive })
            .HasDatabaseName("IX_Product_Category_Active");
    }
}