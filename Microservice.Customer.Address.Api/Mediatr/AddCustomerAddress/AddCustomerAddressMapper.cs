using AutoMapper;

namespace Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;

public class AddCustomerAddressMapper : Profile
{
    public AddCustomerAddressMapper()
    {
        base.CreateMap<AddCustomerAddressRequest, Microservice.Customer.Address.Api.Domain.CustomerAddress>()
            .ForMember(x => x.AddressLine1, act => act.MapFrom(src => src.AddressLine1))
            .ForMember(x => x.AddressLine2, act => act.MapFrom(src => src.AddressLine2))
            .ForMember(x => x.AddressLine3, act => act.MapFrom(src => src.AddressLine3))
            .ForMember(x => x.TownCity, act => act.MapFrom(src => src.TownCity))
            .ForMember(x => x.CountryId, act => act.MapFrom(src => src.CountryId))
            .ForMember(x => x.County, act => act.MapFrom(src => src.County))
            .ForMember(x => x.Postcode, act => act.MapFrom(src => src.Postcode));
    }
}