using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Printbase.Domain.Entities.Products;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Repositories;

public class ProductGroupRepository(ApplicationDbContext dbContext, IMapper mapper) : IProductGroupRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ProductGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductGroupDbEntity>()
            .Include(g => g.Types)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        
        return dbEntity != null ? _mapper.Map<ProductGroup>(dbEntity) : null;
    }

    public async Task<IEnumerable<ProductGroup>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductGroupDbEntity>()
            .Include(g => g.Types)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductGroup>>(dbEntities);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ProductGroupDbEntity>()
            .AnyAsync(g => g.Id == id, cancellationToken);
    }

    public async Task AddAsync(ProductGroup productGroup, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productGroup);
        
        var dbEntity = _mapper.Map<ProductGroupDbEntity>(productGroup);
        await _dbContext.Set<ProductGroupDbEntity>().AddAsync(dbEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductGroup productGroup, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productGroup);
        
        var dbEntity = await _dbContext.Set<ProductGroupDbEntity>()
            .Include(g => g.Types)
            .FirstOrDefaultAsync(g => g.Id == productGroup.Id, cancellationToken);
        
        if (dbEntity == null)
            throw new KeyNotFoundException($"ProductGroup with ID {productGroup.Id} not found");
        
        _mapper.Map(productGroup, dbEntity);
        
        var existingTypeIds = dbEntity.Types.Select(t => t.Id).ToList();
        var updatedTypeIds = productGroup.Types.Select(t => t.Id).ToList();
        
        var typesToRemove = dbEntity.Types.Where(t => !updatedTypeIds.Contains(t.Id)).ToList();
        foreach (var type in typesToRemove)
        {
            dbEntity.Types.Remove(type);
        }
        
        foreach (var type in productGroup.Types)
        {
            if (existingTypeIds.Contains(type.Id)) continue;
            var newType = _mapper.Map<ProductTypeDbEntity>(type);
            dbEntity.Types.Add(newType);
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductGroupDbEntity>()
            .FindAsync([id], cancellationToken);
        
        if (dbEntity == null)
            return;
        
        _dbContext.Set<ProductGroupDbEntity>().Remove(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}