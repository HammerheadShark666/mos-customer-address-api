using Microservice.Customer.Address.Api.Domain;

namespace Microservice.Customer.Address.Api.Data.Repository.Interfaces;

public interface ICountryRepository
{
    Task<List<Country>> AllSortedAsync();
    Task<bool> ExistsAsync(int countryId);
}