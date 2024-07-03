using Microservice.Customer.Address.Api.Data.Contexts;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Api.Data.Repository;

public class CountryRepository : ICountryRepository
{
    private readonly CustomerAddressDbContext _context;

    public CountryRepository(CustomerAddressDbContext context) 
    {
        _context = context;
    }

    public async Task<List<Country>> AllSortedAsync()
    {
        return await _context.Country.OrderBy(country => country.Name).ToListAsync();
    }

    public async Task<bool> ExistsAsync(int countryId)
    {
        return await _context.Country.AnyAsync(x => x.Id.Equals(countryId));
    }
}