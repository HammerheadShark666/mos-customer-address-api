namespace Microservice.Customer.Address.Api.Data.Repository.Interfaces;

public interface ICountryRepository
{
    Task<bool> ExistsAsync(int countryId);
}