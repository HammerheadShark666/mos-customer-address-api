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
        db.SaveChanges();

        return customerAddress;
    }

    public async Task DeleteAsync(Api.Domain.CustomerAddress customerAddress)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Remove(customerAddress);
    }

    public async Task UpdateAsync(Api.Domain.CustomerAddress customerAddress)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        db.CustomerAddress.Update(customerAddress);
    }

    public async Task<List<Api.Domain.CustomerAddress>> ByCustomerAsync(Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress
                        .Where(o => o.CustomerId.Equals(customerId))
                        .Include(e => e.Country) 
                        .ToListAsync();
    }

    public async Task<Api.Domain.CustomerAddress> ByIdAsync(Guid customerId, Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress
                        .Where(o => o.Id.Equals(id) && o.CustomerId.Equals(customerId))
                        .Include(e => e.Country)
                        .SingleOrDefaultAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AnyAsync(x => x.Id.Equals(id));
    }

    public async Task<bool> HasAddressesAsync(Guid customerId)
    {
        await using var db = await _dbContextFactory.CreateDbContextAsync();
        return await db.CustomerAddress.AnyAsync(x => x.CustomerId.Equals(customerId));
    }
}