using Imprink.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Orders.Configuration;

public class OrderAddressConfiguration : EntityBaseConfiguration<OrderAddress>
{
    public override void Configure(EntityTypeBuilder<OrderAddress> builder)
    {
        base.Configure(builder);
            
        builder.Property(oa => oa.OrderId)
            .IsRequired();
                
        builder.Property(oa => oa.Street)
            .IsRequired()
            .HasMaxLength(200);
                
        builder.Property(oa => oa.City)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(oa => oa.State)
            .HasMaxLength(100);
                
        builder.Property(oa => oa.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
                
        builder.Property(oa => oa.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(oa => oa.OrderId)
            .IsUnique()
            .HasDatabaseName("IX_OrderAddress_OrderId");
    }
}