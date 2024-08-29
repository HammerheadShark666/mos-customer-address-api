using Microservice.Customer.Address.Api.Data.Context;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Api.Data.Repository;

public class CustomerAddressRepository(IDbContextFactory<CustomerAddressDbContext> dbContextFactory) : ICustomerAddressRepository
{
    public async Task<Domain.CustomerAddress> AddAsync(Domain.CustomerAddress customerAddress)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.CustomerAddress.AddAsync(customerAddress);
        await db.SaveChangesAsync();

        return customerAddress;
    }

    public async Task DeleteAsync(Domain.CustomerAddress customerAddress)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Remove(customerAddress);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.CustomerAddress customerAddress)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Update(customerAddress);
        await db.SaveChangesAsync();
    }

    public async Task<List<Domain.CustomerAddress>> ByCustomerAsync(Guid customerId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress
                        .AsNoTracking()
                        .Where(o => o.CustomerId.Equals(customerId))
                        .Include(e => e.Country)
                        .ToListAsync();
    }

    public async Task<Domain.CustomerAddress> ByIdAsync(Guid customerId, Guid id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var address = await db.CustomerAddress
                        .AsNoTracking()
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.Country)
                        .SingleOrDefaultAsync() ?? throw new NotFoundException("Customer address not found.");
        return address;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(Guid customerId, Guid addressId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.CustomerId.Equals(customerId) & x.Id.Equals(addressId));
    }

    public async Task<bool> HasAddressesAsync(Guid customerId)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.CustomerId.Equals(customerId));
    }
}