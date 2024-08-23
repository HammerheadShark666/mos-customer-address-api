using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public class GetCustomerAddressesQueryHandler(ICustomerAddressRepository customerAddressRepository,
                                              ILogger<GetCustomerAddressesQueryHandler> logger,
                                              IMapper mapper) : IRequestHandler<GetCustomerAddressesRequest, GetCustomerAddressesResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;
    private ILogger<GetCustomerAddressesQueryHandler> _logger { get; set; } = logger;

    public async Task<GetCustomerAddressesResponse> Handle(GetCustomerAddressesRequest getCustomerAddressesRequest, CancellationToken cancellationToken)
    {
        var customerAddresses = await _customerAddressRepository.ByCustomerAsync(getCustomerAddressesRequest.CustomerId);
        if (customerAddresses == null)
        {
            _logger.LogError($"Customers addresses not found - {getCustomerAddressesRequest.CustomerId }");
            throw new NotFoundException("Customers addresses not found.");
        }

        return _mapper.Map<GetCustomerAddressesResponse>(customerAddresses);
    }
}