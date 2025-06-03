using Imprink.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Orders;

public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
{
    public void Configure(EntityTypeBuilder<OrderStatus> builder)
    {
        builder.HasKey(os => os.Id);
            
        builder.Property(os => os.Id)
            .ValueGeneratedNever();
            
        builder.Property(os => os.Name)
            .IsRequired()
            .HasMaxLength(50);
                
        builder.HasIndex(os => os.Name)
            .IsUnique()
            .HasDatabaseName("IX_OrderStatus_Name");

        builder.HasData(
            new OrderStatus { Id = 0, Name = "Pending" },
            new OrderStatus { Id = 1, Name = "Processing" },
            new OrderStatus { Id = 2, Name = "Completed" },
            new OrderStatus { Id = 3, Name = "Cancelled" }
        );
    }
}