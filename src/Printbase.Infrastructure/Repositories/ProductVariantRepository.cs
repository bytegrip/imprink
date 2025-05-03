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

    public async Task<ProductVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductVariantDbEntity>()
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        
        return dbEntity != null ? _mapper.Map<ProductVariant>(dbEntity) : null;
    }

    public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductVariantDbEntity>()
            .Where(v => v.ProductId == productId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductVariant>>(dbEntities);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ProductVariantDbEntity>()
            .AnyAsync(v => v.Id == id, cancellationToken);
    }

    public async Task AddAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        
        var dbEntity = _mapper.Map<ProductVariantDbEntity>(productVariant);
        await _dbContext.Set<ProductVariantDbEntity>().AddAsync(dbEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        
        var dbEntity = await _dbContext.Set<ProductVariantDbEntity>()
            .FirstOrDefaultAsync(v => v.Id == productVariant.Id, cancellationToken);
        
        if (dbEntity == null)
            throw new KeyNotFoundException($"ProductVariant with ID {productVariant.Id} not found");
        
        _mapper.Map(productVariant, dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductVariantDbEntity>()
            .FindAsync([id], cancellationToken);
        
        if (dbEntity == null)
            return;
        
        _dbContext.Set<ProductVariantDbEntity>().Remove(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}