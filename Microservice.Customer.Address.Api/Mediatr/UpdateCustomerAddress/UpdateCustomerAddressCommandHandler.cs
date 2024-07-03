using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public class UpdateCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                       IMapper mapper) : IRequestHandler<UpdateCustomerAddressRequest, UpdateCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;

    public async Task<UpdateCustomerAddressResponse> Handle(UpdateCustomerAddressRequest addCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Domain.CustomerAddress>(addCustomerAddressRequest);
        await _customerAddressRepository.AddAsync(customer);

        return new UpdateCustomerAddressResponse("Customer address updated.");
    }
}