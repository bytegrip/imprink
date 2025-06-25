using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Include(o => o.OrderAddress)
            .Include(o => o.Product)
            .Include(o => o.ProductVariant)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByUserIdWithDetailsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Include(o => o.OrderAddress)
            .Include(o => o.Product)
            .Include(o => o.ProductVariant)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByMerchantIdAsync(string merchantId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.MerchantId == merchantId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByMerchantIdWithDetailsAsync(string merchantId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.ShippingStatus)
            .Include(o => o.OrderAddress)
            .Include(o => o.Product)
            .Include(o => o.ProductVariant)
            .Include(o => o.User)
            .Where(o => o.MerchantId == merchantId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(int statusId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.OrderStatusId == statusId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByShippingStatusAsync(int shippingStatusId, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Where(o => o.ShippingStatusId == shippingStatusId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        context.Orders.Add(order);
        return Task.FromResult(order);
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        var existingOrder = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == order.Id, cancellationToken);

        if (existingOrder == null)
            throw new InvalidOperationException($"Order with ID {order.Id} not found");

        context.Entry(existingOrder).CurrentValues.SetValues(order);
        return existingOrder;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order == null)
            return false;

        context.Orders.Remove(order);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .AnyAsync(o => o.Id == id, cancellationToken);
    }

    public async Task<bool> IsOrderNumberUniqueAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return !await context.Orders
            .AnyAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<bool> IsOrderNumberUniqueAsync(string orderNumber, Guid excludeOrderId, CancellationToken cancellationToken = default)
    {
        return !await context.Orders
            .AnyAsync(o => o.OrderNumber == orderNumber && o.Id != excludeOrderId, cancellationToken);
    }

    public async Task<string> GenerateOrderNumberAsync(CancellationToken cancellationToken = default)
    {
        string orderNumber;
        bool isUnique;

        do
        {
            // Generate order number format: ORD-YYYYMMDD-XXXXXX (where X is random)
            var datePart = DateTime.UtcNow.ToString("yyyyMMdd");
            var randomPart = Random.Shared.Next(100000, 999999).ToString();
            orderNumber = $"ORD-{datePart}-{randomPart}";

            isUnique = await IsOrderNumberUniqueAsync(orderNumber, cancellationToken);
        }
        while (!isUnique);

        return orderNumber;
    }

    public async Task UpdateStatusAsync(Guid orderId, int statusId, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order != null)
        {
            order.OrderStatusId = statusId;
        }
    }

    public async Task UpdateShippingStatusAsync(Guid orderId, int shippingStatusId, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order != null)
        {
            order.ShippingStatusId = shippingStatusId;
        }
    }

    public async Task AssignMerchantAsync(Guid orderId, string merchantId, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order != null)
        {
            order.MerchantId = merchantId;
        }
    }

    public async Task UnassignMerchantAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order != null)
        {
            order.MerchantId = null;
        }
    }
}