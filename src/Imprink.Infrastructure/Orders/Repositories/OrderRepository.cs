using Imprink.Domain.Entities.Orders;
using Imprink.Domain.Models;
using Imprink.Domain.Repositories.Orders;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Orders.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductVariant)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithAddressAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderAddress)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdFullAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Include(o => o.OrderAddress)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductVariant)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<PagedResult<Order>> GetPagedAsync(
        OrderFilterParameters filterParameters, 
        CancellationToken cancellationToken = default)
    {
        var query = context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filterParameters.UserId))
        {
            query = query.Where(o => o.UserId == filterParameters.UserId);
        }

        if (!string.IsNullOrEmpty(filterParameters.OrderNumber))
        {
            query = query.Where(o => o.OrderNumber.Contains(filterParameters.OrderNumber));
        }

        if (filterParameters.OrderStatusId.HasValue)
        {
            query = query.Where(o => o.OrderStatusId == filterParameters.OrderStatusId.Value);
        }

        if (filterParameters.ShippingStatusId.HasValue)
        {
            query = query.Where(o => o.ShippingStatusId == filterParameters.ShippingStatusId.Value);
        }

        if (filterParameters.StartDate.HasValue)
        {
            query = query.Where(o => o.OrderDate >= filterParameters.StartDate.Value);
        }

        if (filterParameters.EndDate.HasValue)
        {
            query = query.Where(o => o.OrderDate <= filterParameters.EndDate.Value);
        }

        if (filterParameters.MinTotalPrice.HasValue)
        {
            query = query.Where(o => o.TotalPrice >= filterParameters.MinTotalPrice.Value);
        }

        if (filterParameters.MaxTotalPrice.HasValue)
        {
            query = query.Where(o => o.TotalPrice <= filterParameters.MaxTotalPrice.Value);
        }

        query = filterParameters.SortBy.ToLower() switch
        {
            "orderdate" => filterParameters.SortDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderByDescending(o => o.OrderDate)
                : query.OrderBy(o => o.OrderDate),
            "totalprice" => filterParameters.SortDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderByDescending(o => o.TotalPrice)
                : query.OrderBy(o => o.TotalPrice),
            "ordernumber" => filterParameters.SortDirection.Equals("DESC", StringComparison.CurrentCultureIgnoreCase)
                ? query.OrderByDescending(o => o.OrderNumber)
                : query.OrderBy(o => o.OrderNumber),
            _ => query.OrderByDescending(o => o.OrderDate)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filterParameters.PageNumber - 1) * filterParameters.PageSize)
            .Take(filterParameters.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize
        };
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdPagedAsync(
        string userId, 
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByOrderStatusAsync(int orderStatusId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Where(o => o.OrderStatusId == orderStatusId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByShippingStatusAsync(int shippingStatusId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Where(o => o.ShippingStatusId == shippingStatusId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;
        order.ModifiedAt = DateTime.UtcNow;

        context.Orders.Add(order);
        return Task.FromResult(order);
    }

    public Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.ModifiedAt = DateTime.UtcNow;
        context.Orders.Update(order);
        return Task.FromResult(order);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await GetByIdAsync(id, cancellationToken);
        if (order != null)
        {
            context.Orders.Remove(order);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .AnyAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<bool> OrderNumberExistsAsync(string orderNumber, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = context.Orders.Where(o => o.OrderNumber == orderNumber);
        
        if (excludeId.HasValue)
        {
            query = query.Where(o => o.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.OrderStatusId != 5) // Assuming 5 is cancelled status
            .SumAsync(o => o.TotalPrice, cancellationToken);
    }

    public async Task<decimal> GetTotalRevenueByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.OrderStatusId != 5)
            .SumAsync(o => o.TotalPrice, cancellationToken);
    }

    public async Task<int> GetOrderCountByStatusAsync(int orderStatusId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .CountAsync(o => o.OrderStatusId == orderStatusId, cancellationToken);
    }
}