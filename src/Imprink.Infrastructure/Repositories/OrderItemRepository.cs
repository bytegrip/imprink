using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class OrderItemRepository(ApplicationDbContext context) : IOrderItemRepository
{
    public async Task<OrderItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .FirstOrDefaultAsync(oi => oi.Id == id, cancellationToken);
    }

    public async Task<OrderItem?> GetByIdWithProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(oi => oi.Id == id, cancellationToken);
    }

    public async Task<OrderItem?> GetByIdWithVariantAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.ProductVariant)
            .FirstOrDefaultAsync(oi => oi.Id == id, cancellationToken);
    }

    public async Task<OrderItem?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Order)
            .ThenInclude(o => o.User)
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .Include(oi => oi.ProductVariant)
            .FirstOrDefaultAsync(oi => oi.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .OrderBy(oi => oi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByOrderIdWithProductsAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .Include(oi => oi.ProductVariant)
            .Where(oi => oi.OrderId == orderId)
            .OrderBy(oi => oi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.ProductId == productId)
            .OrderByDescending(oi => oi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByProductVariantIdAsync(Guid productVariantId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Order)
            .Where(oi => oi.ProductVariantId == productVariantId)
            .OrderByDescending(oi => oi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetCustomizedItemsAsync(CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => !string.IsNullOrEmpty(oi.CustomizationImageUrl) || !string.IsNullOrEmpty(oi.CustomizationDescription))
            .OrderByDescending(oi => oi.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OrderItem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate)
            .OrderByDescending(oi => oi.Order.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<OrderItem> AddAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
    {
        orderItem.Id = Guid.NewGuid();
        orderItem.CreatedAt = DateTime.UtcNow;
        orderItem.ModifiedAt = DateTime.UtcNow;

        context.OrderItems.Add(orderItem);
        return Task.FromResult(orderItem);
    }

    public Task<IEnumerable<OrderItem>> AddRangeAsync(IEnumerable<OrderItem> orderItems, CancellationToken cancellationToken = default)
    {
        var items = orderItems.ToList();
        var utcNow = DateTime.UtcNow;

        foreach (var item in items)
        {
            item.Id = Guid.NewGuid();
            item.CreatedAt = utcNow;
            item.ModifiedAt = utcNow;
        }

        context.OrderItems.AddRange(items);
        return Task.FromResult<IEnumerable<OrderItem>>(items);
    }

    public Task<OrderItem> UpdateAsync(OrderItem orderItem, CancellationToken cancellationToken = default)
    {
        orderItem.ModifiedAt = DateTime.UtcNow;
        context.OrderItems.Update(orderItem);
        return Task.FromResult(orderItem);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orderItem = await GetByIdAsync(id, cancellationToken);
        if (orderItem != null)
        {
            context.OrderItems.Remove(orderItem);
        }
    }

    public async Task DeleteByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orderItems = await context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .ToListAsync(cancellationToken);

        if (orderItems.Any())
        {
            context.OrderItems.RemoveRange(orderItems);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .AnyAsync(oi => oi.Id == id, cancellationToken);
    }

    public async Task<decimal> GetTotalValueByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Where(oi => oi.OrderId == orderId)
            .SumAsync(oi => oi.TotalPrice, cancellationToken);
    }

    public async Task<int> GetQuantityByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Where(oi => oi.ProductId == productId)
            .SumAsync(oi => oi.Quantity, cancellationToken);
    }

    public async Task<int> GetQuantityByVariantIdAsync(Guid productVariantId, CancellationToken cancellationToken = default)
    {
        return await context.OrderItems
            .Where(oi => oi.ProductVariantId == productVariantId)
            .SumAsync(oi => oi.Quantity, cancellationToken);
    }

    public async Task<Dictionary<Guid, int>> GetProductSalesCountAsync(
        DateTime? startDate = null, 
        DateTime? endDate = null, 
        CancellationToken cancellationToken = default)
    {
        var query = context.OrderItems
            .Include(oi => oi.Order)
            .AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(oi => oi.Order.OrderDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(oi => oi.Order.OrderDate <= endDate.Value);
        }

        return await query
            .GroupBy(oi => oi.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(oi => oi.Quantity) })
            .ToDictionaryAsync(x => x.ProductId, x => x.TotalQuantity, cancellationToken);
    }
}