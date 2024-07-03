using MediatR;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public record UpdateCustomerAddressRequest(Guid Id, Guid CustomerId, string AddressLine1, string AddressLine2, string AddressLine3, string TownCity, string County, string Postcode, int CountryId) : IRequest<UpdateCustomerAddressResponse>;