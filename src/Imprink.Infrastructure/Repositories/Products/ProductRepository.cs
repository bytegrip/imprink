using Imprink.Domain.Entities.Product;
using Imprink.Domain.Models;
using Imprink.Domain.Repositories;
using Imprink.Domain.Repositories.Products;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories.Products;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public async Task<PagedResult<Product>> GetPagedAsync(
        ProductFilterParameters filterParameters, 
        CancellationToken cancellationToken = default)
    {
        var query = context.Products
            .Include(p => p.Category)
            .AsQueryable();

        if (filterParameters.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == filterParameters.IsActive.Value);
        }

        if (!string.IsNullOrEmpty(filterParameters.SearchTerm))
        {
            query = query.Where(
                p => p.Name.Contains(filterParameters.SearchTerm) 
                     || (p.Description != null && p.Description.Contains(filterParameters.SearchTerm)));
        }

        if (filterParameters.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filterParameters.CategoryId.Value);
        }

        if (filterParameters.MinPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice >= filterParameters.MinPrice.Value);
        }

        if (filterParameters.MaxPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice <= filterParameters.MaxPrice.Value);
        }

        if (filterParameters.IsCustomizable.HasValue)
        {
            query = query.Where(p => p.IsCustomizable == filterParameters.IsCustomizable.Value);
        }

        query = filterParameters.SortBy.ToLower() switch
        {
            "price" => filterParameters.SortDirection.Equals("DESC"
                    , StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderByDescending(p => p.BasePrice)
                : query.OrderBy(p => p.BasePrice),
            "name" => filterParameters.SortDirection.Equals("DESC"
                    , StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),
            _ => query.OrderBy(p => p.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filterParameters.PageNumber - 1) * filterParameters.PageSize)
            .Take(filterParameters.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize
        };
    }
    
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetByIdWithVariantsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Include(p => p.ProductVariants.Where(pv => pv.IsActive))
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetByIdWithCategoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductVariants.Where(pv => pv.IsActive))
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetCustomizableAsync(CancellationToken cancellationToken = default)
    {
        return await context.Products
            .Include(p => p.Category)
            .Where(p => p.IsCustomizable && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        product.ModifiedAt = DateTime.UtcNow;
        
        context.Products.Add(product);
        await context.SaveChangesAsync(cancellationToken);
        
        if (product.CategoryId.HasValue)
        {
            await context.Entry(product)
                .Reference(p => p.Category)
                .LoadAsync(cancellationToken);
        }
        
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.ModifiedAt = DateTime.UtcNow;
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetByIdAsync(id, cancellationToken);
        if (product != null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .AnyAsync(p => p.Id == id, cancellationToken);
    }
}