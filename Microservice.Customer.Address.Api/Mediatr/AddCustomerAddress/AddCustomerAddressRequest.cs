using MediatR;

namespace Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;

public record AddCustomerAddressRequest(string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId) : IRequest<AddCustomerAddressResponse>;