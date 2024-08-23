using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Domain;
using Microservice.Customer.Address.Api.Helpers;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

namespace Microservice.Customer.Address.Api.Test.Unit;

[TestFixture]
public class GetCustomerAddressMediatrTests
{
    private readonly Mock<ICustomerAddressRepository> customerAddressRepositoryMock = new();
    private readonly Mock<ICustomerAddressHttpAccessor> customerAddressHttpAccessorMock = new();
    private readonly Mock<ILogger<GetCustomerAddressQueryHandler>> loggerMock = new();
    private readonly ServiceCollection services = new();
    private ServiceProvider serviceProvider;
    private IMediator mediator;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        services.AddValidatorsFromAssemblyContaining<GetCustomerAddressValidator>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCustomerAddressQueryHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddScoped<ICustomerAddressRepository>(sp => customerAddressRepositoryMock.Object);
        services.AddScoped<ICustomerAddressHttpAccessor>(sp => customerAddressHttpAccessorMock.Object);
        services.AddScoped<ILogger<GetCustomerAddressQueryHandler>>(sp => loggerMock.Object);
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
    public async Task Get_customerAddress_return_customerAddress()
    {
        var id = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var addressLine1 = "AddressLine1";
        var addressLine2 = "AddressLine2";
        var addressLine3 = "AddressLine3";
        var townCity = "TownCity";
        var county = "County";
        var postcode = "Postcode";
        var countryId = 1;
        var country = new Country() { Id = 1, Name = "England" };

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

        //customerAddressRepositoryMock
        //        .Setup(x => x.ExistsAsync(id))
        //        .Returns(Task.FromResult(true));

        customerAddressRepositoryMock
                .Setup(x => x.ByIdAsync(customerId, id))
                .Returns(Task.FromResult(customerAddress));

        var getCustomerAddressRequest = new GetCustomerAddressRequest(id, customerId);

        var actualResult = await mediator.Send(getCustomerAddressRequest);
        var expectedResult = new GetCustomerAddressResponse(id, customerId, addressLine1, addressLine2, addressLine3,
                                                                townCity, county, postcode, countryId, country);

        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Id, Is.EqualTo(expectedResult.Id));
            Assert.That(actualResult.CustomerId, Is.EqualTo(expectedResult.CustomerId));
            Assert.That(actualResult.AddressLine1, Is.EqualTo(expectedResult.AddressLine1));
            Assert.That(actualResult.AddressLine2, Is.EqualTo(expectedResult.AddressLine2));
            Assert.That(actualResult.AddressLine3, Is.EqualTo(expectedResult.AddressLine3));
            Assert.That(actualResult.TownCity, Is.EqualTo(expectedResult.TownCity));
            Assert.That(actualResult.County, Is.EqualTo(expectedResult.County));
            Assert.That(actualResult.Postcode, Is.EqualTo(expectedResult.Postcode));
            Assert.That(actualResult.CountryId, Is.EqualTo(expectedResult.CountryId));
            Assert.That(actualResult.Country, Is.EqualTo(expectedResult.Country));
        });
    }

    [Test]
    public void Get_customerAddress_return_exception()
    {
        var customerId = Guid.NewGuid();
        var customerAddressId = Guid.NewGuid();

        customerAddressRepositoryMock
                .Setup(x => x.ExistsAsync(customerAddressId))
                .Returns(Task.FromResult(false));

        customerAddressHttpAccessorMock.Setup(x => x.CustomerId)
            .Returns(customerAddressId);

        var getCustomerAddressRequest = new GetCustomerAddressRequest(customerAddressId, customerId);

        var validationException = Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await mediator.Send(getCustomerAddressRequest);
        });

        Assert.That(validationException.Message, Is.EqualTo("Customer address not found."));
    }
}