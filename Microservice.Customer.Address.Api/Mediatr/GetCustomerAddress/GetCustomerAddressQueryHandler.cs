using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public class GetCustomerAddressQueryHandler(ICustomerAddressRepository customerAddressRepository,
                                            ILogger<GetCustomerAddressQueryHandler> logger,
                                            IMapper mapper) : IRequestHandler<GetCustomerAddressRequest, GetCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;
    private ILogger<GetCustomerAddressQueryHandler> _logger { get; set; } = logger;

    public async Task<GetCustomerAddressResponse> Handle(GetCustomerAddressRequest getCustomerAddressRequest, CancellationToken cancellationToken)
    {
        var customerAddress = await _customerAddressRepository.ByIdAsync(getCustomerAddressRequest.CustomerId, getCustomerAddressRequest.Id);
        if (customerAddress == null)
        {
            _logger.LogError($"Customer address not found - {getCustomerAddressRequest.Id}");
            throw new NotFoundException("Customer address not found.");
        }

        return _mapper.Map<GetCustomerAddressResponse>(customerAddress);
    }
}