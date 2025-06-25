using System.Text.Json;
using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration;

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
            
        builder.Property(o => o.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(o => o.ProductId)
            .IsRequired();
            
        builder.Property(o => o.OrderStatusId)
            .IsRequired();
            
        builder.Property(o => o.ShippingStatusId)
            .IsRequired();
            
        builder.Property(o => o.OrderNumber)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(o => o.Notes)
            .HasMaxLength(1000);

        builder.Property(o => o.MerchantId)
            .HasMaxLength(450);

        builder.Property(o => o.ComposingImageUrl)
            .HasMaxLength(1000);

        builder.Property(o => o.OriginalImageUrls)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<string>())
            .HasColumnType("nvarchar(max)");

        builder.Property(o => o.CustomizationImageUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(o => o.CustomizationDescription)
            .IsRequired()
            .HasMaxLength(2000);

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

        builder.HasOne<User>()
            .WithMany(u => u.MerchantOrders)
            .HasForeignKey(o => o.MerchantId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.ProductVariant)
            .WithMany(pv => pv.Orders)
            .HasForeignKey(o => o.ProductVariantId)
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

        builder.HasIndex(o => o.MerchantId)
            .HasDatabaseName("IX_Order_MerchantId");

        builder.HasIndex(o => o.ProductId)
            .HasDatabaseName("IX_Order_ProductId");

        builder.HasIndex(o => o.ProductVariantId)
            .HasDatabaseName("IX_Order_ProductVariantId");
            
        builder.HasIndex(o => new { o.UserId, o.OrderDate })
            .HasDatabaseName("IX_Order_User_Date");

        builder.HasIndex(o => new { o.MerchantId, o.OrderDate })
            .HasDatabaseName("IX_Order_Merchant_Date");

        builder.HasIndex(o => new { o.ProductId, o.OrderDate })
            .HasDatabaseName("IX_Order_Product_Date");
    }
}
