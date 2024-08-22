using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microservice.Customer.Address.Api.Data.Contexts;
using Microservice.Customer.Address.Api.Data.Repository;
using Microservice.Customer.Address.Api.Data.Repository.Interfaces;
using Microservice.Customer.Address.Api.Helpers;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.Helpers.Swagger;
using Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;
using Microservice.Customer.Address.Api.Middleware;
using Microservice.Customer.Address.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Microservice.Customer.Address.Api.Extensions;

public static class IServiceCollectionExtensions
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AddCustomerAddressMapper)));
    }

    public static void ConfigureExceptionHandling(this IServiceCollection services)
    {
        services.AddTransient<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureJwt(this IServiceCollection services)
    {
        services.AddJwtAuthentication();
    }

    public static void ConfigureDI(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddSingleton<ICustomerAddressHttpAccessor, CustomerAddressHttpAccessor>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
    }

    public static void ConfigureDatabaseContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContextFactory<CustomerAddressDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString(Constants.DatabaseConnectionString),
            options => options.EnableRetryOnFailure()));
    }

    public static void ConfigureMediatr(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddCustomerAddressValidator>();
        services.AddMediatR(_ => _.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
    }

    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<SwaggerDefaultValues>();
            options.SupportNonNullableReferenceTypes();
        });
    }
}