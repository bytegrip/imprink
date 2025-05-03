using Microsoft.EntityFrameworkCore;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Database;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<ProductDbEntity> Products { get; set; } = null!;
    public DbSet<ProductVariantDbEntity> ProductVariants { get; set; } = null!;
    public DbSet<ProductTypeDbEntity> ProductTypes { get; set; } = null!;
    public DbSet<ProductGroupDbEntity> ProductGroups { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProductDbEntity>()
            .HasMany(p => p.Variants)
            .WithOne(v => v.Product)
            .HasForeignKey(v => v.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductDbEntity>()
            .HasOne(p => p.Type)
            .WithMany(t => t.Products)
            .HasForeignKey(p => p.TypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductTypeDbEntity>()
            .HasOne(t => t.Group)
            .WithMany(g => g.Types)
            .HasForeignKey(t => t.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductDbEntity>()
            .Property(p => p.Discount)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<ProductVariantDbEntity>()
            .Property(v => v.Price)
            .HasPrecision(18, 2);
        
        modelBuilder.Entity<ProductVariantDbEntity>()
            .Property(v => v.Discount)
            .HasPrecision(18, 2);
    }
}