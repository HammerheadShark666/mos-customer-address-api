using AutoMapper;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public class UpdateCustomerAddressMapper : Profile
{
    public UpdateCustomerAddressMapper()
    {
        base.CreateMap<UpdateCustomerAddressRequest, Microservice.Customer.Address.Api.Domain.CustomerAddress>();
    }
}