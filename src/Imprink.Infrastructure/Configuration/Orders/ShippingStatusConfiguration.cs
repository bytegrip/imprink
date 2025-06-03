using Imprink.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Orders;

public class ShippingStatusConfiguration : IEntityTypeConfiguration<ShippingStatus>
{
    public void Configure(EntityTypeBuilder<ShippingStatus> builder)
    {
        builder.HasKey(ss => ss.Id);
            
        builder.Property(ss => ss.Id)
            .ValueGeneratedNever();
            
        builder.Property(ss => ss.Name)
            .IsRequired()
            .HasMaxLength(50);
                
        builder.HasIndex(ss => ss.Name)
            .IsUnique()
            .HasDatabaseName("IX_ShippingStatus_Name");

        builder.HasData(
            new ShippingStatus { Id = 0, Name = "Prepping" },
            new ShippingStatus { Id = 1, Name = "Packaging" },
            new ShippingStatus { Id = 2, Name = "Shipped" },
            new ShippingStatus { Id = 3, Name = "Delivered" }
        );
    }
}