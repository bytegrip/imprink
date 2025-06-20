using Imprink.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure.Users.Configuration;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
        
        builder.Property(ur => ur.UserId)
            .IsRequired()
            .HasMaxLength(450);
            
        builder.Property(ur => ur.RoleId)
            .IsRequired();

        builder.HasIndex(ur => ur.UserId)
            .HasDatabaseName("IX_UserRole_UserId");
            
        builder.HasIndex(ur => ur.RoleId)
            .HasDatabaseName("IX_UserRole_RoleId");

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}