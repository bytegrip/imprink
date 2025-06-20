using Imprink.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Imprink.Infrastructure;

public class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
            
        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()");
            
        builder.Property(e => e.CreatedAt)
            .IsRequired();
                
        builder.Property(e => e.ModifiedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");
                
        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(450);
                
        builder.Property(e => e.ModifiedBy)
            .IsRequired()
            .HasMaxLength(450);

        builder.HasIndex(e => e.CreatedAt)
            .HasDatabaseName($"IX_{typeof(T).Name}_CreatedAt");
                
        builder.HasIndex(e => e.ModifiedAt)
            .HasDatabaseName($"IX_{typeof(T).Name}_ModifiedAt");
                
        builder.HasIndex(e => e.CreatedBy)
            .HasDatabaseName($"IX_{typeof(T).Name}_CreatedBy");
    }
}