using Microservice.Customer.Address.Api.Data.Context;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Microservice.Customer.Address.Api.Data.Repository;

public class CountryRepository(CustomerAddressDbContext context) : ICountryRepository
{
    public async Task<bool> ExistsAsync(int countryId)
    {
        return await context.Country.AsNoTracking().AnyAsync(x => x.Id.Equals(countryId));
    }
}