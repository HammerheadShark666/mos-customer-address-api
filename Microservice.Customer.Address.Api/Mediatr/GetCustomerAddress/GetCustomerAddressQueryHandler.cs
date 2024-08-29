using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public class GetCustomerAddressQueryHandler(ICustomerAddressRepository customerAddressRepository,
                                            ILogger<GetCustomerAddressQueryHandler> logger,
                                            IMapper mapper) : IRequestHandler<GetCustomerAddressRequest, GetCustomerAddressResponse>
{
    public async Task<GetCustomerAddressResponse> Handle(GetCustomerAddressRequest getCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = await customerAddressRepository.ByIdAsync(getCustomerAddressRequest.CustomerId, getCustomerAddressRequest.Id);
        if (customerAddress == null)
        {
            logger.LogError("Customer address not found - {getCustomerAddressRequest.Id}", getCustomerAddressRequest.Id);
            throw new NotFoundException("Customer address not found.");
        }

        return mapper.Map<GetCustomerAddressResponse>(customerAddress);
    }
}