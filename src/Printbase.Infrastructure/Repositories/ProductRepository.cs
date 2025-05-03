using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Printbase.Domain.Entities.Products;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Repositories;

public class ProductRepository(ApplicationDbContext dbContext, IMapper mapper) : IProductRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductDbEntity>()
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return dbEntity != null ? _mapper.Map<Product>(dbEntity) : null;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductDbEntity>()
            .Include(p => p.Variants)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<Product>>(dbEntities);
    }

    public async Task<IEnumerable<Product>> GetByTypeIdAsync(Guid typeId, CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductDbEntity>()
            .Include(p => p.Variants)
            .Where(p => p.TypeId == typeId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<Product>>(dbEntities);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ProductDbEntity>()
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        var dbEntity = _mapper.Map<ProductDbEntity>(product);
        await _dbContext.Set<ProductDbEntity>().AddAsync(dbEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        var dbEntity = await _dbContext.Set<ProductDbEntity>()
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);
        
        if (dbEntity == null)
            throw new KeyNotFoundException($"Product with ID {product.Id} not found");
        
        _mapper.Map(product, dbEntity);
        
        var existingVariantIds = dbEntity.Variants.Select(v => v.Id).ToList();
        var updatedVariantIds = product.Variants.Select(v => v.Id).ToList();
        
        var variantsToRemove = dbEntity.Variants.Where(v => !updatedVariantIds.Contains(v.Id)).ToList();
        foreach (var variant in variantsToRemove)
        {
            dbEntity.Variants.Remove(variant);
        }
        
        foreach (var variant in product.Variants)
        {
            if (existingVariantIds.Contains(variant.Id)) continue;
            var newVariant = _mapper.Map<ProductVariantDbEntity>(variant);
            dbEntity.Variants.Add(newVariant);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductDbEntity>()
            .FindAsync([id], cancellationToken);
        
        if (dbEntity == null) return;
        
        _dbContext.Set<ProductDbEntity>().Remove(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}