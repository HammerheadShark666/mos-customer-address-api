using AutoMapper;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public class GetCustomerAddressesMapper : Profile
{
    public GetCustomerAddressesMapper()
    {
        base.CreateMap<List<Address.Api.Domain.CustomerAddress>, GetCustomerAddressesResponse>()
            .ForCtorParam(nameof(GetCustomerAddressesResponse.CustomerAddresses),
                    opt => opt.MapFrom(src => src));

        base.CreateMap<Address.Api.Domain.CustomerAddress, GetCustomerAddresses.CustomerAddress>()
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.Id),
                    opt => opt.MapFrom(src => src.Id))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.AddressLine1),
                    opt => opt.MapFrom(src => src.AddressLine1))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.AddressLine2),
                    opt => opt.MapFrom(src => src.AddressLine2))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.AddressLine3),
                    opt => opt.MapFrom(src => src.AddressLine3))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.TownCity),
                    opt => opt.MapFrom(src => src.TownCity))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.County),
                    opt => opt.MapFrom(src => src.County))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.Postcode),
                    opt => opt.MapFrom(src => src.Postcode))
            .ForCtorParam(nameof(GetCustomerAddresses.CustomerAddress.CountryId),
                    opt => opt.MapFrom(src => src.CountryId)); 
    }
}