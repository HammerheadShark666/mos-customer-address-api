using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;

public class AddCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                              IMapper mapper,
                                              ICustomerAddressHttpAccessor customerAddressHttpAccessor) : IRequestHandler<AddCustomerAddressRequest, AddCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;
    private ICustomerAddressHttpAccessor _customerAddressHttpAccessor { get; set; } = customerAddressHttpAccessor;

    public async Task<AddCustomerAddressResponse> Handle(AddCustomerAddressRequest addCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = await _customerAddressRepository.AddAsync(GetCustomerAddress(addCustomerAddressRequest));
        return new AddCustomerAddressResponse(customerAddress.Id);
    }

    private Domain.CustomerAddress GetCustomerAddress(AddCustomerAddressRequest addCustomerAddressRequest)
    {
        var customerAddress = _mapper.Map<Domain.CustomerAddress>(addCustomerAddressRequest);

        customerAddress.Id = Guid.NewGuid();
        customerAddress.CustomerId = _customerAddressHttpAccessor.CustomerId;

        return customerAddress;
    }
}