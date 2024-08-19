using FluentValidation;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;

namespace Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;

public class UpdateCustomerAddressValidator : AbstractValidator<UpdateCustomerAddressRequest>
{
    private readonly ICustomerAddressRepository _customerAddressRepository;
    private readonly ICountryRepository _countryRepository;

    public UpdateCustomerAddressValidator(ICustomerAddressRepository customerAddressRepository, ICountryRepository countryRepository)
    {
        _customerAddressRepository = customerAddressRepository;
        _countryRepository = countryRepository;

        RuleFor(address => address).MustAsync(async (address, cancellation) =>
        {
            return await CustomerAddressExists(address.CustomerId, address.Id);
        })
        .WithMessage(x => $"The customer address does not exists.");

        RuleFor(address => address.AddressLine1)
                   .NotEmpty().WithMessage("Address line 1 is required.")
                   .Length(1, 50).WithMessage("Address line 1 is length between 1 and 50.");

        RuleFor(address => address.AddressLine2)
                   .Length(1, 50).WithMessage("Address line 2 is length between 1 and 50.");

        RuleFor(address => address.AddressLine3)
                   .Length(1, 50).WithMessage("Address line 3 is length between 1 and 50.");

        RuleFor(address => address.TownCity)
              .NotEmpty().WithMessage("Town/City is required.")
              .Length(1, 50).WithMessage("Town/City length is between 1 and 50.");

        RuleFor(address => address.County)
              .Length(1, 50).WithMessage("County length is between 1 and 50.");

        RuleFor(address => address.Postcode)
              .NotEmpty().WithMessage("Postcode is required.")
              .Length(6, 8).WithMessage("Postcode length is between 6 and 8.");

        RuleFor(address => address).MustAsync(async (address, cancellation) =>
        {
            return await CountryExists(address.CountryId);
        })
        .WithMessage(x => $"The country does not exists.");
    }

    protected async Task<bool> CountryExists(int id)
    {
        return await _countryRepository.ExistsAsync(id);
    }

    protected async Task<bool> CustomerAddressExists(Guid customerId, Guid addressId)
    {
        return await _customerAddressRepository.ExistsAsync(customerId, addressId);
    }
}