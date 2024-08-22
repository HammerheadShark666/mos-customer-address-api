using Microservice.Customer.Address.Api.Data.Contexts;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Data.Repository;

public class CustomerAddressRepository(IDbContextFactory<CustomerAddressDbContext> dbContextFactory) : ICustomerAddressRepository
{
    public IDbContextFactory<CustomerAddressDbContext> _dbContextFactory { get; set; } = dbContextFactory;

    public async Task<Api.Domain.CustomerAddress> AddAsync(Api.Domain.CustomerAddress customerAddress)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await db.CustomerAddress.AddAsync(customerAddress);
        await db.SaveChangesAsync();

        return customerAddress;
    }

    public async Task DeleteAsync(Api.Domain.CustomerAddress customerAddress)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Remove(customerAddress);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Api.Domain.CustomerAddress customerAddress)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Update(customerAddress);
        await db.SaveChangesAsync();
    }

    public async Task<List<Api.Domain.CustomerAddress>> ByCustomerAsync(Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress
                        .AsNoTracking()
                        .Where(o => o.CustomerId.Equals(customerId))
                        .Include(e => e.Country)
                        .ToListAsync();
    }

    public async Task<Api.Domain.CustomerAddress> ByIdAsync(Guid customerId, Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress
                        .AsNoTracking()
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.Country)
                        .SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> ExistsAsync(Guid customerId, Guid addressId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.CustomerId.Equals(customerId) & x.Id.Equals(addressId));
    }

    public async Task<bool> HasAddressesAsync(Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AsNoTracking().AnyAsync(x => x.CustomerId.Equals(customerId));
    }
}