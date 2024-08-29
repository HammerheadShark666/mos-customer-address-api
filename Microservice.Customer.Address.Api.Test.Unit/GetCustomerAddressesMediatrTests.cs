using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Domain;
using Microservice.Customer.Address.Api.Helpers;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Address.Api.Test.Unit;

[TestFixture]
public class GetCustomerAddressesMediatrTests
{
    private readonly Mock<ICustomerAddressRepository> customerAddressRepositoryMock = new();
    private readonly Mock<ICustomerAddressHttpAccessor> customerAddressHttpAccessorMock = new();
    private readonly Mock<ILogger<GetCustomerAddressesQueryHandler>> loggerMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

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
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCustomerAddressQueryHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerAddressRepository>(sp => customerAddressRepositoryMock.Object);
        services.AddScoped<ICustomerAddressHttpAccessor>(sp => customerAddressHttpAccessorMock.Object);
        services.AddScoped<ILogger<GetCustomerAddressesQueryHandler>>(sp => loggerMock.Object);
        services.AddAutoMapper(Assembly.GetAssembly(typeof(GetCustomerAddressMapper)));

        serviceProvider = services.BuildServiceProvider();
        mediator = serviceProvider.GetRequiredService<IMediator>();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        services.Clear();
        serviceProvider.Dispose();
    }

    [Test]
    public async Task Get_customerAddresses_return_customerAddress()
    {
        customerAddressRepositoryMock
                .Setup(x => x.ByCustomerAsync(customerId))
                .Returns(Task.FromResult(GetCustomerAddressesReturnedFromDb()));

        var getCustomerAddressesRequest = new GetCustomerAddressesRequest(customerId);
        var actualResult = await mediator.Send(getCustomerAddressesRequest);
        var expectedResult = GetExpectedResponse();

        Assert.That(actualResult.CustomerAddresses, Has.Count.EqualTo(2));

        var firstActualCustomerAddress = actualResult.CustomerAddresses.ElementAt(0);
        var firstExpectedCustomerAddress = expectedResult.CustomerAddresses.ElementAt(0);

        Assert.Multiple(() =>
        {
            Assert.That(firstActualCustomerAddress.Id, Is.EqualTo(firstExpectedCustomerAddress.Id));
            Assert.That(firstActualCustomerAddress.AddressLine1, Is.EqualTo(firstExpectedCustomerAddress.AddressLine1));
            Assert.That(firstActualCustomerAddress.AddressLine2, Is.EqualTo(firstExpectedCustomerAddress.AddressLine2));
            Assert.That(firstActualCustomerAddress.AddressLine3, Is.EqualTo(firstExpectedCustomerAddress.AddressLine3));
            Assert.That(firstActualCustomerAddress.TownCity, Is.EqualTo(firstExpectedCustomerAddress.TownCity));
            Assert.That(firstActualCustomerAddress.County, Is.EqualTo(firstExpectedCustomerAddress.County));
            Assert.That(firstActualCustomerAddress.Postcode, Is.EqualTo(firstExpectedCustomerAddress.Postcode));
            Assert.That(firstActualCustomerAddress.CountryId, Is.EqualTo(firstExpectedCustomerAddress.CountryId));
            Assert.That(firstActualCustomerAddress.Country, Is.EqualTo(firstExpectedCustomerAddress.Country));
        });
    }

    [Test]
    public void Get_customerAddress_return_exception()
    {
        var customerId = Guid.NewGuid();

        var getCustomerAddressRequest = new GetCustomerAddressesRequest(customerId);

        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(getCustomerAddressRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo("Customers addresses not found."));
    }

    private List<Customer.Address.Api.Domain.CustomerAddress> GetCustomerAddressesReturnedFromDb()
    {
        var customerAddress = new Customer.Address.Api.Domain.CustomerAddress()
        {
            Id = id,
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

        return [customerAddress, customerAddress];
    }

    private GetCustomerAddressesResponse GetExpectedResponse()
    {
        return new GetCustomerAddressesResponse([
            new Customer.Address.Api.MediatR.GetCustomerAddresses.CustomerAddress(id,
                       addressLine1, addressLine2, addressLine3, townCity, county, postcode, countryId, country),
            new Customer.Address.Api.MediatR.GetCustomerAddresses.CustomerAddress(id,
                       addressLine1, addressLine2, addressLine3, townCity, county, postcode, countryId, country)]);
    }
}