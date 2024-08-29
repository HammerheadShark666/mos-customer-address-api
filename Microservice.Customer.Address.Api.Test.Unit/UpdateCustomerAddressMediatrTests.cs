using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Domain;
using Microservice.Customer.Address.Api.Helpers;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Address.Api.Test.Unit;

[TestFixture]
public class UpdateCustomerAddressMediatrTests
{
    private readonly Mock<ICustomerAddressRepository> customerAddressRepositoryMock = new();
    private readonly Mock<ICountryRepository> countryRepositoryMock = new();
    private readonly Mock<ICustomerAddressHttpAccessor> customerAddressHttpAccessorMock = new();
    private readonly Mock<ILogger<UpdateCustomerAddressCommandHandler>> iLogger = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;
    private Guid customerAddressId;

    private readonly Guid id = Guid.NewGuid();
    private readonly Guid customerId = Guid.NewGuid();
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
        services.AddValidatorsFromAssemblyContaining<UpdateCustomerAddressValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdateCustomerAddressCommandHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerAddressRepository>(sp => customerAddressRepositoryMock.Object);
        services.AddScoped<ICountryRepository>(sp => countryRepositoryMock.Object);
        services.AddScoped<ICustomerAddressHttpAccessor>(sp => customerAddressHttpAccessorMock.Object);
        services.AddScoped<ILogger<UpdateCustomerAddressCommandHandler>>(sp => iLogger.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(UpdateCustomerAddressMapper)));

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
    public async Task CustomerAddress_updated_return_success_message()
    {
        var customerAddress = new CustomerAddress()
        {
            Id = id,
            CustomerId = customerAddressId,
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
                .Setup(x => x.UpdateAsync(customerAddress));

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerId, id))
                .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.ByIdAsync(customerId, id))
                .Returns(Task.FromResult(customerAddress));

        var updateCustomerAddressRequest = new UpdateCustomerAddressRequest(id, customerId, addressLine1, addressLine2, addressLine3, townCity, county, postcode, countryId);

        var actualResult = await mediator.Send(updateCustomerAddressRequest);
        var expectedResult = "Customer address updated.";

        Assert.That(actualResult.Message, Is.EqualTo(expectedResult));
    }

    [Test]
    public void CustomerAddress_not_updated_address_not_found_return_exception_fail_message()
    {
        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerId, id))
                .Returns(Task.FromResult(false));

        var updateCustomerAddressRequest = new UpdateCustomerAddressRequest(id, customerId, addressLine1, addressLine2, addressLine3, townCity, county, postcode, countryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(updateCustomerAddressRequest);
        });

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Errors.Count, Is.EqualTo(1));
            Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("The customer address does not exist."));
        });
    }

    [Test]
    public void CustomerAddress_not_updated_as_country_id_does_not_exists_return_exception_fail_message()
    {
        int invalidCountryId = 100;

        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(false));

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerId, id))
                .Returns(Task.FromResult(true));

        var updateCustomerAddressRequest = new UpdateCustomerAddressRequest(id, customerId, addressLine1, addressLine2, addressLine3, townCity, county, postcode, invalidCountryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(updateCustomerAddressRequest);
        });

        Assert.Multiple(() =>
        {
            Assert.That(validationException.Errors.Count, Is.EqualTo(1));
            Assert.That(validationException.Errors.ElementAt(0).ErrorMessage, Is.EqualTo("The country does not exists."));
        });
    }

    [Test]
    public void CustomerAddress_not_updated_missing_required_data_return_exception_fail_message()
    {
        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerId, id))
                .Returns(Task.FromResult(true));

        var updateCustomerAddressRequest = new UpdateCustomerAddressRequest(id, customerId, "", addressLine2, addressLine3, "", county, "", countryId);

        var validationException = Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await mediator.Send(updateCustomerAddressRequest);
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
    public void CustomerAddress_not_updated_invalid_data_return_exception_fail_message()
    {
        var invalidString = "CustomerAddressCustomerAddressCustomerAddressCustomerAddressCustomerAddress";

        countryRepositoryMock
                .Setup(x => x.ExistsAsync(countryId))
                .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerId, id))
                .Returns(Task.FromResult(true));

        var addCustomerAddressRequest = new UpdateCustomerAddressRequest(id, customerId, invalidString, invalidString, invalidString, invalidString, invalidString, invalidString, countryId);

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