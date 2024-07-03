using AutoMapper;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public class GetCustomerAddressMapper : Profile
{
    public GetCustomerAddressMapper()
    {
        base.CreateMap<Address.Api.Domain.CustomerAddress, GetCustomerAddressResponse>();
    }
}