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

    public async Task<ProductGroup?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductGroupDbEntity> query = _dbContext.ProductGroups;
        
        if (includeRelations) query = query.Include(g => g.Types);
        
        var dbEntity = await query.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        
        return dbEntity == null ? null : _mapper.Map<ProductGroup>(dbEntity);
    }

    public async Task<IEnumerable<ProductGroup>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductGroupDbEntity> query = _dbContext.ProductGroups;
        
        if (includeRelations) query = query.Include(g => g.Types);
        
        var dbEntities = await query.ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductGroup>>(dbEntities);
    }

    public async Task<ProductGroup> AddAsync(ProductGroup group, CancellationToken cancellationToken = default)
    {
        var dbEntity = _mapper.Map<ProductGroupDbEntity>(group);
        
        _dbContext.ProductGroups.Add(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductGroup>(dbEntity);
    }

    public async Task<ProductGroup> UpdateAsync(ProductGroup group, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbContext.ProductGroups
            .Include(g => g.Types)
            .FirstOrDefaultAsync(g => g.Id == group.Id, cancellationToken);
            
        if (existingEntity == null) throw new KeyNotFoundException($"ProductGroup with ID {group.Id} not found");
        
        _mapper.Map(group, existingEntity);
        
        existingEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductGroup>(existingEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ProductGroups.FindAsync([id], cancellationToken);
        
        if (entity == null) return false;
        
        _dbContext.ProductGroups.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductGroups.AnyAsync(g => g.Id == id, cancellationToken);
    }
}