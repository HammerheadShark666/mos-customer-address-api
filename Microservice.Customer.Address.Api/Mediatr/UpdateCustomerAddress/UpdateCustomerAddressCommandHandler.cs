using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public class UpdateCustomerAddressCommandHandler(ICustomerAddressRepository customerAddressRepository,
                                                 ILogger<UpdateCustomerAddressCommandHandler> logger,
                                                 IMapper mapper) : IRequestHandler<UpdateCustomerAddressRequest, UpdateCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    ILogger<UpdateCustomerAddressCommandHandler> _logger { get; set; } = logger;
    private IMapper _mapper { get; set; } = mapper;

    public async Task<UpdateCustomerAddressResponse> Handle(UpdateCustomerAddressRequest updateCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var existingCustomerAddress = await _customerAddressRepository.ByIdAsync(updateCustomerAddressRequest.CustomerId, updateCustomerAddressRequest.Id);
        if (existingCustomerAddress == null)
        {
            _logger.LogError("Customer address not found: Id - {0}, Customer - {1}.", updateCustomerAddressRequest.Id, updateCustomerAddressRequest.CustomerId);
            throw new NotFoundException("Customer Address not found.");
        }

        existingCustomerAddress = _mapper.Map(updateCustomerAddressRequest, existingCustomerAddress);

        var customerAddress = _mapper.Map<Domain.CustomerAddress>(updateCustomerAddressRequest);
        await _customerAddressRepository.UpdateAsync(customerAddress);

        return new UpdateCustomerAddressResponse("Customer address updated.");
    }
}