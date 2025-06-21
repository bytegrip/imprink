using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration;

public class CategoryConfiguration : EntityBaseConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(c => c.Description)
            .HasMaxLength(2000);
            
        builder.Property(c => c.ImageUrl)
            .HasMaxLength(500);
            
        builder.Property(c => c.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);
            
        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.Property(c => c.ParentCategoryId)
            .IsRequired(false);
        
        builder.Property(c => c.CreatedAt)
            .IsRequired(false);
        
        builder.Property(c => c.CreatedBy)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedAt)
            .IsRequired(false);
        
        builder.Property(c => c.ModifiedBy)
            .IsRequired(false);

        builder.HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.Name)
            .HasDatabaseName("IX_Category_Name");
            
        builder.HasIndex(c => c.IsActive)
            .HasDatabaseName("IX_Category_IsActive");
            
        builder.HasIndex(c => c.ParentCategoryId)
            .HasDatabaseName("IX_Category_ParentCategoryId");
            
        builder.HasIndex(c => new { c.ParentCategoryId, c.SortOrder })
            .HasDatabaseName("IX_Category_Parent_SortOrder");
            
        builder.HasIndex(c => new { c.IsActive, c.SortOrder })
            .HasDatabaseName("IX_Category_Active_SortOrder");

        builder.HasData(
            new Category
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Textile",
                Description = "Textile and fabric-based products",
                IsActive = true,
                SortOrder = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "system@printbase.com",
                ModifiedBy = "system@printbase.com"
            },
            new Category
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Hard Surfaces",
                Description = "Products for hard surface printing",
                IsActive = true,
                SortOrder = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "system@printbase.com",
                ModifiedBy = "system@printbase.com"
            },
            new Category
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Paper",
                Description = "Paper-based printing products",
                IsActive = true,
                SortOrder = 3,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                ModifiedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "system@printbase.com",
                ModifiedBy = "system@printbase.com"
            }
        );
    }
}