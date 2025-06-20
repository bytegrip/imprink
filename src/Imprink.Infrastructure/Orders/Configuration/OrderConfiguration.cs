using Imprink.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Orders.Configuration;

public class OrderConfiguration : EntityBaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            base.Configure(builder);
            
            builder.Property(o => o.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(o => o.OrderDate)
                .IsRequired();
                
            builder.Property(o => o.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
                
            builder.Property(o => o.OrderStatusId)
                .IsRequired();
                
            builder.Property(o => o.ShippingStatusId)
                .IsRequired();
                
            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(o => o.Notes)
                .HasMaxLength(1000);

            builder.HasOne(o => o.OrderStatus)
                .WithMany(os => os.Orders)
                .HasForeignKey(o => o.OrderStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.ShippingStatus)
                .WithMany(ss => ss.Orders)
                .HasForeignKey(o => o.ShippingStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.OrderAddress)
                .WithOne(oa => oa.Order)
                .HasForeignKey<OrderAddress>(oa => oa.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(o => o.UserId)
                .HasDatabaseName("IX_Order_UserId");
                
            builder.HasIndex(o => o.OrderNumber)
                .IsUnique()
                .HasDatabaseName("IX_Order_OrderNumber");
                
            builder.HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Order_OrderDate");
                
            builder.HasIndex(o => o.OrderStatusId)
                .HasDatabaseName("IX_Order_OrderStatusId");
                
            builder.HasIndex(o => o.ShippingStatusId)
                .HasDatabaseName("IX_Order_ShippingStatusId");
                
            builder.HasIndex(o => new { o.UserId, o.OrderDate })
                .HasDatabaseName("IX_Order_User_Date");
        }
    }