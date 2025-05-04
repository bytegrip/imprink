using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Printbase.Domain.Entities.Products;
using Printbase.Domain.Repositories;
using Printbase.Infrastructure.Database;
using Printbase.Infrastructure.DbEntities.Products;

namespace Printbase.Infrastructure.Repositories;

public class ProductTypeRepository(ApplicationDbContext dbContext, IMapper mapper) : IProductTypeRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<ProductType?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductTypeDbEntity> query = _dbContext.ProductTypes;
        
        if (includeRelations)
        {
            query = query
                .Include(t => t.Group)
                .Include(t => t.Products);
        }
        
        var dbEntity = await query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        return dbEntity == null ? null : _mapper.Map<ProductType>(dbEntity);
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductTypeDbEntity> query = _dbContext.ProductTypes;
        
        if (includeRelations)
        {
            query = query
                .Include(t => t.Group)
                .Include(t => t.Products);
        }
        
        var dbEntities = await query.ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductType>>(dbEntities);
    }

    public async Task<IEnumerable<ProductType>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.ProductTypes
            .Where(t => t.GroupId == groupId)
            .ToListAsync(cancellationToken);
            
        return _mapper.Map<IEnumerable<ProductType>>(dbEntities);
    }

    public async Task<ProductType> AddAsync(ProductType type, CancellationToken cancellationToken = default)
    {
        var dbEntity = _mapper.Map<ProductTypeDbEntity>(type);
        
        dbEntity.Group = await _dbContext.ProductGroups.FindAsync([type.GroupId], cancellationToken)
            ?? throw new InvalidOperationException($"ProductGroup with ID {type.GroupId} not found");
        
        _dbContext.ProductTypes.Add(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductType>(dbEntity);
    }

    public async Task<ProductType> UpdateAsync(ProductType type, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbContext.ProductTypes
            .Include(t => t.Products)
            .FirstOrDefaultAsync(t => t.Id == type.Id, cancellationToken);
            
        if (existingEntity == null) throw new KeyNotFoundException($"ProductType with ID {type.Id} not found");
        
        _mapper.Map(type, existingEntity);
        
        if (existingEntity.GroupId != type.GroupId)
        {
            existingEntity.Group = await _dbContext.ProductGroups.FindAsync([type.GroupId], cancellationToken)
                ?? throw new InvalidOperationException($"ProductGroup with ID {type.GroupId} not found");
            existingEntity.GroupId = type.GroupId;
        }
        
        existingEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ProductType>(existingEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ProductTypes.FindAsync([id], cancellationToken);
        
        if (entity == null)
        {
            return false;
        }
        
        _dbContext.ProductTypes.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.ProductTypes.AnyAsync(t => t.Id == id, cancellationToken);
    }
}