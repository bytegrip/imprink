using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Printbase.Domain.Entities.Products;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Repositories;

public class ProductVariantRepository(ApplicationDbContext dbContext, IMapper mapper) : IProductVariantRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ProductVariant?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductVariantDbEntity> query = _dbContext.ProductVariants;
        
        if (includeRelations)
            query = query.Include(v => v.Product)
                        .ThenInclude(p => p.Type);
        
        var dbEntity = await query.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        
        return dbEntity == null ? null : _mapper.Map<ProductVariant>(dbEntity);
    }

    public async Task<IEnumerable<ProductVariant>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductVariantDbEntity> query = _dbContext.ProductVariants;
        
        if (includeRelations)
            query = query.Include(v => v.Product)
                        .ThenInclude(p => p.Type);
        
        var dbEntities = await query.ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductVariant>>(dbEntities);
    }

    public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.ProductVariants
            .Where(v => v.ProductId == productId)
            .ToListAsync(cancellationToken);
            
        return _mapper.Map<IEnumerable<ProductVariant>>(dbEntities);
    }

    public async Task<ProductVariant> AddAsync(ProductVariant variant, CancellationToken cancellationToken = default)
    {
        var dbEntity = _mapper.Map<ProductVariantDbEntity>(variant);
        
        dbEntity.Product = await _dbContext.Products.FindAsync([variant.ProductId], cancellationToken)
            ?? throw new InvalidOperationException($"Product with ID {variant.ProductId} not found");
        
        _dbContext.ProductVariants.Add(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductVariant>(dbEntity);
    }

    public async Task<ProductVariant> UpdateAsync(ProductVariant variant, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbContext.ProductVariants
            .FindAsync([variant.Id], cancellationToken);
            
        if (existingEntity == null) throw new KeyNotFoundException($"ProductVariant with ID {variant.Id} not found");
        
        _mapper.Map(variant, existingEntity);
        
        if (existingEntity.ProductId != variant.ProductId)
        {
            existingEntity.Product = await _dbContext.Products.FindAsync([variant.ProductId], cancellationToken)
                ?? throw new InvalidOperationException($"Product with ID {variant.ProductId} not found");
            existingEntity.ProductId = variant.ProductId;
        }
        
        existingEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductVariant>(existingEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ProductVariants.FindAsync([id], cancellationToken);
        
        if (entity == null) return false;
        
        _dbContext.ProductVariants.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductVariants.AnyAsync(v => v.Id == id, cancellationToken);
    }
}