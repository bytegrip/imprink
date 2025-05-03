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

    public async Task<ProductType?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductTypeDbEntity>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        return dbEntity != null ? _mapper.Map<ProductType>(dbEntity) : null;
    }

    public async Task<IEnumerable<ProductType>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductTypeDbEntity>()
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductType>>(dbEntities);
    }

    public async Task<IEnumerable<ProductType>> GetByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
    {
        var dbEntities = await _dbContext.Set<ProductTypeDbEntity>()
            .Where(t => t.GroupId == groupId)
            .ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<ProductType>>(dbEntities);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<ProductTypeDbEntity>()
            .AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AddAsync(ProductType productType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productType);
        
        var dbEntity = _mapper.Map<ProductTypeDbEntity>(productType);
        await _dbContext.Set<ProductTypeDbEntity>().AddAsync(dbEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductType productType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(productType);
        
        var dbEntity = await _dbContext.Set<ProductTypeDbEntity>()
            .FirstOrDefaultAsync(t => t.Id == productType.Id, cancellationToken);
        
        if (dbEntity == null)
            throw new KeyNotFoundException($"ProductType with ID {productType.Id} not found");
        
        _mapper.Map(productType, dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var dbEntity = await _dbContext.Set<ProductTypeDbEntity>()
            .FindAsync([id], cancellationToken);
        
        if (dbEntity == null)
            return;
        
        _dbContext.Set<ProductTypeDbEntity>().Remove(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}