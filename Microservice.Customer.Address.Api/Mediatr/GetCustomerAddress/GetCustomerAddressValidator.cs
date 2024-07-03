using FluentValidation;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;

public class GetCustomerAddressValidator : AbstractValidator<GetCustomerAddressRequest>
{
    private readonly ICustomerAddressRepository _customerAddressRepository;

    public GetCustomerAddressValidator(ICustomerAddressRepository customerAddressRepository)
    {
        _customerAddressRepository = customerAddressRepository;
    }
}