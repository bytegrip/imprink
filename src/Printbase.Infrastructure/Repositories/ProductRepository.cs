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

    public async Task<Product?> GetByIdAsync(Guid id, bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductDbEntity> query = _dbContext.Products;
        
        if (includeRelations)
        {
            query = query
                .Include(p => p.Type)
                    .ThenInclude(t => t.Group)
                .Include(p => p.Variants);
        }
        
        var dbEntity = await query.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return dbEntity == null ? null : _mapper.Map<Product>(dbEntity);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        IQueryable<ProductDbEntity> query = _dbContext.Products;
        
        if (includeRelations)
        {
            query = query
                .Include(p => p.Type)
                    .ThenInclude(t => t.Group)
                .Include(p => p.Variants);
        }
        
        var dbEntities = await query.ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<Product>>(dbEntities);
    }

    public async Task<IEnumerable<Product>> GetByTypeIdAsync(Guid typeId, bool includeRelations = false, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Products.Where(p => p.TypeId == typeId);
        
        if (includeRelations)
        {
            query = query
                .Include(p => p.Type)
                    .ThenInclude(t => t.Group)
                .Include(p => p.Variants);
        }
        
        var dbEntities = await query.ToListAsync(cancellationToken);
        
        return _mapper.Map<IEnumerable<Product>>(dbEntities);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        var dbEntity = _mapper.Map<ProductDbEntity>(product);
        
        dbEntity.Type = await _dbContext.ProductTypes.FindAsync([product.TypeId], cancellationToken)
            ?? throw new InvalidOperationException($"ProductType with ID {product.TypeId} not found");
        
        _dbContext.Products.Add(dbEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<Product>(dbEntity);
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbContext.Products
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(p => p.Id == product.Id, cancellationToken);
            
        if (existingEntity == null) throw new KeyNotFoundException($"Product with ID {product.Id} not found");
        
        _mapper.Map(product, existingEntity);
        
        if (existingEntity.TypeId != product.TypeId)
        {
            existingEntity.Type = await _dbContext.ProductTypes.FindAsync([product.TypeId], cancellationToken)
                ?? throw new InvalidOperationException($"ProductType with ID {product.TypeId} not found");
            existingEntity.TypeId = product.TypeId;
        }
        
        existingEntity.UpdatedAt = DateTime.UtcNow;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<Product>(existingEntity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Products.FindAsync([id], cancellationToken);
        
        if (entity == null) return false;
        
        _dbContext.Products.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AnyAsync(p => p.Id == id, cancellationToken);
    }
}