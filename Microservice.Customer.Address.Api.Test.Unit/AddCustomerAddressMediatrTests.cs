using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Domain;
using Microservice.Customer.Address.Api.Helpers;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Address.Api.Test.Unit;

[TestFixture]
public class AddCustomerAddressMediatrTests
{
    private readonly Mock<ICustomerAddressRepository> customerAddressRepositoryMock = new();
    private readonly Mock<ICountryRepository> countryRepositoryMock = new();
    private readonly Mock<ICustomerAddressHttpAccessor> customerAddressHttpAccessorMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;
    private Guid customerAddressId;

    private readonly string addressLine1 = "AddressLine1";
    private readonly string addressLine2 = "AddressLine2";
    private readonly string addressLine3 = "AddressLine3";
    private readonly string townCity = "TownCity";
    private readonly string county = "County";
    private readonly string postcode = "Postcode";
    private readonly int countryId = 1;
    private readonly Country country = new() { Id = 1, Name = "England" };

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerAddressValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddCustomerAddressCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerAddressRepository>(sp => customerAddressRepositoryMock.Object);
        services.AddScoped<ICountryRepository>(sp => countryRepositoryMock.Object);
        services.AddScoped<ICustomerAddressHttpAccessor>(sp => customerAddressHttpAccessorMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerAddressMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [SetUp]
    public void SetUp()
    {
        customerAddressId = Guid.NewGuid();

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerAddressId))
                .Returns(Task.FromResult(true));
    }

    [Test]
    public async Task CustomerAddress_added_return_success_message()
    {
        var customerId = Guid.NewGuid();
        var savedCustomerAddress = new CustomerAddress()
        {
            Id = Guid.NewGuid(),
            CustomerId = customerId,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            AddressLine3 = addressLine3,
            TownCity = townCity,
            County = county,
            Postcode = postcode,
            CountryId = countryId,
            Country = country
        };

        customerAddressHttpAccessorMock.Setup(x => x.CustomerId)
                .Returns(customerId);

        countryRepositoryMock.Setup(x => x.ExistsAsync(country.Id))
                .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<CustomerAddress>()))
                .Returns(Task.FromResult(savedCustomerAddress));

        var addCustomerAddressRequest = new AddCustomerAddressRequest(addressLine1, addressLine2, addressLine3, townCity, county, postcode, countryId);

        var actualResult = await mediator.Send(addCustomerAddressRequest);

        Assert.That(actualResult.Id, !Is.Empty);
    }

    [Test]
    public void CustomerAddress_not_added_as_country_id_does_not_exists_return_exception_fail_message()
    {
        int invalidCountryId = 100;

        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(false));

        var addCustomerAddressRequest = new AddCustomerAddressRequest(addressLine1, addressLine2, addressLine3, townCity, county, postcode, invalidCountryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerAddressRequest);
        });

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Errors.Count, Is.EqualTo(1));
            Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("The country does not exists."));
        });
    }

    [Test]
    public void CustomerAddress_not_added_missing_required_data_return_exception_fail_message()
    {
        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(true));

        var addCustomerAddressRequest = new AddCustomerAddressRequest("", addressLine2, addressLine3, "", county, "", countryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerAddressRequest);
        });

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Errors.Count, Is.EqualTo(6));
            Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Address line 1 is required."));
            Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Address line 1 is length between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("Town/City is required."));
            Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Town/City length is between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("Postcode is required."));
            Assert.That(validationException.Errors.ElementAt(5).ErrorMessage, Is.EqualTo("Postcode length is between 6 and 8."));
        });
    }

    [Test]
    public void CustomerAddress_not_added_invalid_data_return_exception_fail_message()
    {
        var invalidString = "CustomerAddressCustomerAddressCustomerAddressCustomerAddressCustomerAddress";

        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(true));

        var addCustomerAddressRequest = new AddCustomerAddressRequest(invalidString, invalidString, invalidString, invalidString, invalidString, invalidString, countryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(addCustomerAddressRequest);
        });

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Errors.Count, Is.EqualTo(6));
            Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("Address line 1 is length between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(1).ErrorMessage, Is.EqualTo("Address line 2 is length between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(2).ErrorMessage, Is.EqualTo("Address line 3 is length between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(3).ErrorMessage, Is.EqualTo("Town/City length is between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(4).ErrorMessage, Is.EqualTo("County length is between 1 and 50."));
            Assert.That(validationException.Errors.ElementAt(5).ErrorMessage, Is.EqualTo("Postcode length is between 6 and 8."));
        });
    }
}