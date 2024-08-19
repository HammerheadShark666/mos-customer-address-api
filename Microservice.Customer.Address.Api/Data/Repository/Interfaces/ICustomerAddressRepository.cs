namespace Microservice.Customer.Address.Api.Data.Repository.Interfaces;

public interface ICustomerAddressRepository
{
    Task<Domain.CustomerAddress> AddAsync(Domain.CustomerAddress customerAddress);
    Task UpdateAsync(Domain.CustomerAddress customerAddress);
    Task DeleteAsync(Domain.CustomerAddress customerAddress);
    Task<List<Domain.CustomerAddress>> ByCustomerAsync(Guid customerId);
    Task<Domain.CustomerAddress> ByIdAsync(Guid customerId, Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsAsync(Guid customerId, Guid addressId);
    Task<bool> HasAddressesAsync(Guid customerId);
}