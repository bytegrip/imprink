using Imprink.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Configuration.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id)
            .HasMaxLength(450)
            .ValueGeneratedNever();
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.Nickname)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.EmailVerified)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(u => u.FullName)
            .HasMaxLength(100);
            
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);
            
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
            
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_User_Email");
            
        builder.HasIndex(u => u.IsActive)
            .HasDatabaseName("IX_User_IsActive");

        builder.HasMany(u => u.Addresses)
            .WithOne()
            .HasForeignKey(a => a.UserId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(u => u.DefaultAddress);
        builder.Ignore(u => u.Roles);
    }
}