using Imprink.Domain.Entities;
using Imprink.Domain.Repositories;
using Imprink.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Imprink.Infrastructure.Repositories;

public class AddressRepository(ApplicationDbContext context) : IAddressRepository
{
    public async Task<IEnumerable<Address>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AddressType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Address>> GetActiveByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AddressType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Address?> GetDefaultByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault && a.IsActive, cancellationToken);
    }

    public async Task<IEnumerable<Address>> GetByUserIdAndTypeAsync(string userId, string addressType, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .Where(a => a.UserId == userId && a.AddressType == addressType && a.IsActive)
            .OrderByDescending(a => a.IsDefault)
            .ToListAsync(cancellationToken);
    }

    public async Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Address?> GetByIdAndUserIdAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, cancellationToken);
    }

    public async Task<Address> AddAsync(Address address, CancellationToken cancellationToken = default)
    {
        if (address.IsDefault)
        {
            await UnsetDefaultAddressesAsync(address.UserId, cancellationToken);
        }

        context.Addresses.Add(address);
        return address;
    }

    public async Task<Address> UpdateAsync(Address address, CancellationToken cancellationToken = default)
    {
        var existingAddress = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == address.Id, cancellationToken);

        if (existingAddress == null)
            throw new InvalidOperationException($"Address with ID {address.Id} not found");

        if (address.IsDefault && !existingAddress.IsDefault)
        {
            await UnsetDefaultAddressesAsync(address.UserId, cancellationToken);
        }

        context.Entry(existingAddress).CurrentValues.SetValues(address);
        return existingAddress;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var address = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (address == null)
            return false;

        context.Addresses.Remove(address);
        return true;
    }

    public async Task<bool> DeleteByUserIdAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        var address = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, cancellationToken);

        if (address == null)
            return false;

        context.Addresses.Remove(address);
        return true;
    }

    public async Task SetDefaultAddressAsync(string userId, Guid addressId, CancellationToken cancellationToken = default)
    {
        await UnsetDefaultAddressesAsync(userId, cancellationToken);

        var address = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId, cancellationToken);

        if (address != null)
        {
            address.IsDefault = true;
        }
    }

    public async Task DeactivateAddressAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        var address = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId, cancellationToken);

        if (address != null)
        {
            address.IsActive = false;
            if (address.IsDefault)
            {
                address.IsDefault = false;
            }
        }
    }

    public async Task ActivateAddressAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        var address = await context.Addresses
            .FirstOrDefaultAsync(a => a.Id == addressId, cancellationToken);

        if (address != null)
        {
            address.IsActive = true;
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> IsUserAddressAsync(Guid id, string userId, CancellationToken cancellationToken = default)
    {
        return await context.Addresses
            .AnyAsync(a => a.Id == id && a.UserId == userId, cancellationToken);
    }

    private async Task UnsetDefaultAddressesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var defaultAddresses = await context.Addresses
            .Where(a => a.UserId == userId && a.IsDefault)
            .ToListAsync(cancellationToken);

        foreach (var addr in defaultAddresses)
        {
            addr.IsDefault = false;
        }
    }
}