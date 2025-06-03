using Imprink.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Users;

public class AddressConfiguration : EntityBaseConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);
            
        builder.Property(a => a.UserId)
            .IsRequired()
            .HasMaxLength(450);
                
        builder.Property(a => a.AddressType)
            .IsRequired()
            .HasMaxLength(50);
                
        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(200);
                
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(a => a.State)
            .HasMaxLength(100);
                
        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
                
        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);
                
        builder.Property(a => a.IsDefault)
            .IsRequired()
            .HasDefaultValue(false);
                
        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => a.UserId)
            .HasDatabaseName("IX_Address_UserId");
                
        builder.HasIndex(a => new { a.UserId, a.AddressType })
            .HasDatabaseName("IX_Address_User_Type");
                
        builder.HasIndex(a => new { a.UserId, a.IsDefault })
            .HasDatabaseName("IX_Address_User_Default");
    }
}