using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public class UpdateCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                                 ILogger<UpdateCustomerAddressCommandHandler> logger,
                                                 IMapper mapper) : IRequestHandler<UpdateCustomerAddressRequest, UpdateCustomerAddressResponse>
{
    public async Task<UpdateCustomerAddressResponse> Handle(UpdateCustomerAddressRequest updateCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var existingCustomerAddress = await customerAddressRepository.ByIdAsync(updateCustomerAddressRequest.CustomerId, updateCustomerAddressRequest.Id);
        if (existingCustomerAddress == null)
        {
            logger.LogError("Customer address not found: Id - {updateCustomerAddressRequest.Id}, Customer - {updateCustomerAddressRequest.CustomerId}.", updateCustomerAddressRequest.Id, updateCustomerAddressRequest.CustomerId);
            throw new NotFoundException("Customer Address not found.");
        }

        var customerAddress = mapper.Map<Domain.CustomerAddress>(updateCustomerAddressRequest);
        await customerAddressRepository.UpdateAsync(customerAddress);

        return new UpdateCustomerAddressResponse("Customer address updated.");
    }
}