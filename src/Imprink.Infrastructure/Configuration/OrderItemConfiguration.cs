using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration;

public class OrderItemConfiguration : EntityBaseConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            base.Configure(builder);
            
            builder.Property(oi => oi.OrderId)
                .IsRequired();
                
            builder.Property(oi => oi.ProductId)
                .IsRequired();
                
            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasDefaultValue(1);
                
            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(oi => oi.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(oi => oi.CustomizationImageUrl)
                .HasMaxLength(500);
                
            builder.Property(oi => oi.CustomizationDescription)
                .HasMaxLength(2000);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.ProductVariant)
                .WithMany(pv => pv.OrderItems)
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(oi => oi.OrderId)
                .HasDatabaseName("IX_OrderItem_OrderId");
                
            builder.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("IX_OrderItem_ProductId");
                
            builder.HasIndex(oi => oi.ProductVariantId)
                .HasDatabaseName("IX_OrderItem_ProductVariantId");
                
            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
                .HasDatabaseName("IX_OrderItem_Order_Product");
        }
    }