using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public class GetCustomerAddressesQueryHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper) : IRequestHandler<GetCustomerAddressesRequest, GetCustomerAddressesResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;

    public async Task<GetCustomerAddressesResponse> Handle(GetCustomerAddressesRequest request, CancellationToken cancellationToken)
    {
        var customerAddresses = await _customerAddressRepository.ByCustomerAsync(request.CustomerId);
        if (customerAddresses == null)
        {
            throw new NotFoundException("Customers addresses not found.");
        }

        return _mapper.Map<GetCustomerAddressesResponse>(customerAddresses);
    }
}