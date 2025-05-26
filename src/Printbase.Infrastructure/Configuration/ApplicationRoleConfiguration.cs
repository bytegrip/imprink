using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Printbase.Infrastructure.Configuration;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(r => r.Description)
                .HasMaxLength(500);
                
            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");
                
            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasIndex(r => r.IsActive)
                .HasDatabaseName("IX_ApplicationRole_IsActive");

            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            builder.HasData(
                new ApplicationRole 
                { 
                    Id = "1", 
                    Name = "Administrator", 
                    NormalizedName = "ADMINISTRATOR",
                    Description = "Full system access",
                    CreatedAt = seedDate,
                    IsActive = true
                },
                new ApplicationRole 
                { 
                    Id = "2", 
                    Name = "Customer", 
                    NormalizedName = "CUSTOMER",
                    Description = "Standard customer access",
                    CreatedAt = seedDate,
                    IsActive = true
                },
                new ApplicationRole 
                { 
                    Id = "3", 
                    Name = "OrderManager", 
                    NormalizedName = "ORDERMANAGER",
                    Description = "Manage orders and fulfillment",
                    CreatedAt = seedDate,
                    IsActive = true
                },
                new ApplicationRole 
                { 
                    Id = "4", 
                    Name = "ProductManager", 
                    NormalizedName = "PRODUCTMANAGER",
                    Description = "Manage products and inventory",
                    CreatedAt = seedDate,
                    IsActive = true
                }
            );
        }
    }