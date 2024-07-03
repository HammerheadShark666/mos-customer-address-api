using MediatR;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public record GetCustomerAddressRequest(Guid Id, Guid CustomerId) : IRequest<GetCustomerAddressResponse>;