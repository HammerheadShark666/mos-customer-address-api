using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;

public class AddCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                              IMapper mapper,
                                              ICustomerAddressHttpAccessor customerAddressHttpAccessor) : IRequestHandler<AddCustomerAddressRequest, AddCustomerAddressResponse>
{
    public async Task<AddCustomerAddressResponse> Handle(AddCustomerAddressRequest addCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = await customerAddressRepository.AddAsync(GetCustomerAddress(addCustomerAddressRequest));
        return new AddCustomerAddressResponse(customerAddress.Id);
    }

    private Domain.CustomerAddress GetCustomerAddress(AddCustomerAddressRequest addCustomerAddressRequest)
    {
        var customerAddress = mapper.Map<Domain.CustomerAddress>(addCustomerAddressRequest);

        customerAddress.Id = Guid.NewGuid();
        customerAddress.CustomerId = customerAddressHttpAccessor.CustomerId;

        return customerAddress;
    }
}