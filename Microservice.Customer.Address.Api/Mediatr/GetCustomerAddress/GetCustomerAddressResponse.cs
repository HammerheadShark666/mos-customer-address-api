using Microservice.Customer.Address.Api.Domain;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;
 
public record GetCustomerAddressResponse(Guid Id, Guid CustomerId, string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId, Country Country);