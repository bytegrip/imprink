using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class OrderAddressRepository(ApplicationDbContext context) : IOrderAddressRepository
{

    public async Task<OrderAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderAddresses
            .Include(oa => oa.Order)
            .FirstOrDefaultAsync(oa => oa.Id == id, cancellationToken);
    }

    public async Task<OrderAddress?> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.OrderAddresses
            .Include(oa => oa.Order)
            .FirstOrDefaultAsync(oa => oa.OrderId == orderId, cancellationToken);
    }

    public async Task<IEnumerable<OrderAddress>> GetByOrderIdsAsync(IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default)
    {
        return await context.OrderAddresses
            .Include(oa => oa.Order)
            .Where(oa => orderIds.Contains(oa.OrderId))
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderAddress> AddAsync(OrderAddress orderAddress, CancellationToken cancellationToken = default)
    {
        var entry = await context.OrderAddresses.AddAsync(orderAddress, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<OrderAddress> UpdateAsync(OrderAddress orderAddress, CancellationToken cancellationToken = default)
    {
        context.OrderAddresses.Update(orderAddress);
        await context.SaveChangesAsync(cancellationToken);
        return orderAddress;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var orderAddress = await context.OrderAddresses.FindAsync([id], cancellationToken);
        if (orderAddress == null)
            return false;

        context.OrderAddresses.Remove(orderAddress);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var orderAddress = await context.OrderAddresses.FirstOrDefaultAsync(oa => oa.OrderId == orderId, cancellationToken);
        if (orderAddress == null)
            return false;

        context.OrderAddresses.Remove(orderAddress);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.OrderAddresses.AnyAsync(oa => oa.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await context.OrderAddresses.AnyAsync(oa => oa.OrderId == orderId, cancellationToken);
    }
}