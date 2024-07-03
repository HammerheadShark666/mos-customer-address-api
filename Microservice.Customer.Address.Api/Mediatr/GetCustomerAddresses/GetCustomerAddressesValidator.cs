using FluentValidation;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;

public class GetCustomerAddressesValidator : AbstractValidator<GetCustomerAddressesRequest>
{
    private readonly ICustomerAddressRepository _customerAddressRepository;

    public GetCustomerAddressesValidator(ICustomerAddressRepository customerAddressRepository)
    {
        _customerAddressRepository = customerAddressRepository;
    }
}