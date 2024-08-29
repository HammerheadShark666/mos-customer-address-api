using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public class GetCustomerAddressesQueryHandler(ICustomerAddressRepository customerAddressRepository,
                                              ILogger<GetCustomerAddressesQueryHandler> logger,
                                              IMapper mapper) : IRequestHandler<GetCustomerAddressesRequest, GetCustomerAddressesResponse>
{
    public async Task<GetCustomerAddressesResponse> Handle(GetCustomerAddressesRequest getCustomerAddressesRequest, CancellationToken cancellationToken)
    {
        var customerAddresses = await customerAddressRepository.ByCustomerAsync(getCustomerAddressesRequest.CustomerId);
        if (customerAddresses == null)
        {
            logger.LogError("Customers addresses not found - {getCustomerAddressesRequest.CustomerId}", getCustomerAddressesRequest.CustomerId);
            throw new NotFoundException("Customers addresses not found.");
        }

        return mapper.Map<GetCustomerAddressesResponse>(customerAddresses);
    }
}