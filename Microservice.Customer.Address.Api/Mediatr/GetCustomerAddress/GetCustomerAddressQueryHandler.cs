using AutoMapper;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Exceptions;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public class GetCustomerAddressQueryHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper) : IRequestHandler<GetCustomerAddressRequest, GetCustomerAddressResponse>
{
    private ICustomerAddressRepository _customerAddressRepository { get; set; } = customerAddressRepository;
    private IMapper _mapper { get; set; } = mapper;
     
    public async Task<GetCustomerAddressResponse> Handle(GetCustomerAddressRequest request, CancellationToken cancellationToken)
    {  
        var customerAddress = await _customerAddressRepository.ByIdAsync(request.CustomerId, request.Id); 
        if (customerAddress == null)
        {
            throw new NotFoundException("Customer address not found.");
        }

        return _mapper.Map<GetCustomerAddressResponse>(customerAddress); 
    }
}