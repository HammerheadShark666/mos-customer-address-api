using MediatR;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public record GetCustomerAddressesRequest(Guid CustomerId) : IRequest<GetCustomerAddressesResponse>;