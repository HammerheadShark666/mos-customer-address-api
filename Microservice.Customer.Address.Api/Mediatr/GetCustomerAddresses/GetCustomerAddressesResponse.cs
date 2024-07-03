using Microservice.Customer.Address.Api.Domain;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;
 
public record GetCustomerAddressesResponse(List<CustomerAddress> CustomerAddresses);

public record CustomerAddress(Guid Id, string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId, Country Country);