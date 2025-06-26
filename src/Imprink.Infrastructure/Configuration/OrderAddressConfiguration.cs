using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration;

public class OrderAddressConfiguration : EntityBaseConfiguration<OrderAddress>
{
    public override void Configure(EntityTypeBuilder<OrderAddress> builder)
    {
        base.Configure(builder);
            
        builder.Property(oa => oa.OrderId)
            .IsRequired();

        builder.Property(oa => oa.AddressType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(oa => oa.FirstName)
            .HasMaxLength(100);

        builder.Property(oa => oa.LastName)
            .HasMaxLength(100);

        builder.Property(oa => oa.Company)
            .HasMaxLength(200);
                
        builder.Property(oa => oa.AddressLine1)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(oa => oa.AddressLine2)
            .HasMaxLength(200);

        builder.Property(oa => oa.ApartmentNumber)
            .HasMaxLength(20);

        builder.Property(oa => oa.BuildingNumber)
            .HasMaxLength(20);

        builder.Property(oa => oa.Floor)
            .HasMaxLength(20);
                
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

        builder.Property(oa => oa.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(oa => oa.Instructions)
            .HasMaxLength(500);

        builder.HasIndex(oa => oa.OrderId)
            .IsUnique()
            .HasDatabaseName("IX_OrderAddress_OrderId");
    }
}