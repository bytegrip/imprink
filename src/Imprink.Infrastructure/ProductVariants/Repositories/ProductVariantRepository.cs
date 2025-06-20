using Imprink.Domain.Entities.Products;
using Imprink.Domain.Repositories.Products;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.ProductVariants.Repositories;

public class ProductVariantRepository(ApplicationDbContext context) : IProductVariantRepository
{
    public async Task<ProductVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .FirstOrDefaultAsync(pv => pv.Id == id, cancellationToken);
    }

    public async Task<ProductVariant?> GetByIdWithProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .Include(pv => pv.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(pv => pv.Id == id, cancellationToken);
    }

    public async Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(pv => pv.Sku == sku, cancellationToken);
    }

    public async Task<IEnumerable<ProductVariant>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .Include(pv => pv.Product)
            .Where(pv => pv.ProductId == productId)
            .OrderBy(pv => pv.Size)
            .ThenBy(pv => pv.Color)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductVariant>> GetActiveByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .Include(pv => pv.Product)
            .Where(pv => pv.ProductId == productId && pv.IsActive)
            .OrderBy(pv => pv.Size)
            .ThenBy(pv => pv.Color)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductVariant>> GetInStockByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .Include(pv => pv.Product)
            .Where(pv => pv.ProductId == productId && pv.IsActive && pv.StockQuantity > 0)
            .OrderBy(pv => pv.Size)
            .ThenBy(pv => pv.Color)
            .ToListAsync(cancellationToken);
    }

    public Task<ProductVariant> AddAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
    {
        productVariant.Id = Guid.NewGuid();
        productVariant.CreatedAt = DateTime.UtcNow;
        productVariant.ModifiedAt = DateTime.UtcNow;

        context.ProductVariants.Add(productVariant);
        return Task.FromResult(productVariant);
    }

    public Task<ProductVariant> UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
    {
        productVariant.ModifiedAt = DateTime.UtcNow;
        context.ProductVariants.Update(productVariant);
        return Task.FromResult(productVariant);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var productVariant = await GetByIdAsync(id, cancellationToken);
        if (productVariant != null)
        {
            context.ProductVariants.Remove(productVariant);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.ProductVariants
            .AnyAsync(pv => pv.Id == id, cancellationToken);
    }

    public async Task<bool> SkuExistsAsync(string sku, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = context.ProductVariants.Where(pv => pv.Sku == sku);
        
        if (excludeId.HasValue)
        {
            query = query.Where(pv => pv.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task UpdateStockQuantityAsync(Guid id, int quantity, CancellationToken cancellationToken = default)
    {
        var productVariant = await GetByIdAsync(id, cancellationToken);
        if (productVariant != null)
        {
            productVariant.StockQuantity = quantity;
            productVariant.ModifiedAt = DateTime.UtcNow;
        }
    }
}