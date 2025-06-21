using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedNever();
            
        builder.Property(r => r.RoleName)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => r.RoleName)
            .IsUnique()
            .HasDatabaseName("IX_Role_RoleName");

        builder.HasData(
            new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), RoleName = "Merchant" },
            new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), RoleName = "Admin" }
        );
    }
}