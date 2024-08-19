using Asp.Versioning;
using MediatR;
using Microservice.Customer.Address.Api.Extensions;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;
using Microservice.Customer.Address.Api.MediatR.UpdateCustomerAddress;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Net;

namespace Microservice.Customer.Address.Api.Endpoints;

public static class Endpoints
{
    public static void ConfigureRoutes(this WebApplication app, ConfigurationManager configuration)
    {
        var customerGroup = app.MapGroup("v{version:apiVersion}/customer-addresses").WithTags("customerAddresses");

        customerGroup.MapGet("/{id}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async ([FromRoute] Guid id,  [FromServices] IMediator mediator, ICustomerAddressHttpAccessor customerAddressHttpAccessor) =>
        {
            var getCustomerResponse = await mediator.Send(new GetCustomerAddressRequest(id, customerAddressHttpAccessor.CustomerId));
            return Results.Ok(getCustomerResponse);
        })
        .Produces<GetCustomerAddressResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("GetCustomerAddress")
        .WithApiVersionSet(app.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get a customer address based on id.",
            Description = "Gets a customers address based on its id.",
            Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers Address" } }
        });

        customerGroup.MapGet("/logged-in", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async ([FromServices] IMediator mediator, ICustomerAddressHttpAccessor customerAddressHttpAccessor) =>
        {
            var result = await mediator.Send(new GetCustomerAddressesRequest(customerAddressHttpAccessor.CustomerId));
            return result == null ? Results.NotFound() : Results.Ok(result);
        })
        .Produces<GetCustomerAddressesResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("GetCustomersAddresses")
        .WithApiVersionSet(app.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Get customers addresses.",
            Description = "Gets customers addresses.",
            Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers Addresses" } }
        });

        customerGroup.MapPost("/add", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async([FromBody] AddCustomerAddressRequest addCustomerAddressRequest, [FromServices] IMediator mediator) =>
        { 
            var addCustomerAddressResponse = await mediator.Send(addCustomerAddressRequest);
            return Results.Ok(addCustomerAddressResponse);
        })
        .Accepts<AddCustomerAddressRequest>("application/json")
        .Produces<AddCustomerAddressResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("AddCustomerAddress")
        .WithApiVersionSet(app.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Add a customer address.",
            Description = "Adds a customer address.",
            Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers Address" } }
        });

        customerGroup.MapPut("/update", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async ([FromBody] UpdateCustomerAddressRequest updateCustomerAddressRequest, [FromServices] IMediator mediator) =>
        {
            var updateCustomerAddressResponse = await mediator.Send(updateCustomerAddressRequest);
            return Results.Ok(updateCustomerAddressResponse);
        })
        .Accepts<UpdateCustomerAddressRequest>("application/json")
        .Produces<UpdateCustomerAddressResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("UpdateCustomerAddress")
        .WithApiVersionSet(app.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Update a customer address.",
            Description = "Updates a customer address.",
            Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers Address" } }
        });







        customerGroup.MapGet("/test", () =>
        {
            throw new Exception("Test Exception");
            //var getCustomerResponse = await mediator.Send(new GetCustomerAddressRequest(id, customerAddressHttpAccessor.CustomerId));
            //return Results.Ok(getCustomerResponse);
        })
        .Produces<GetCustomerAddressResponse>((int)HttpStatusCode.OK)
        .Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        .WithName("TestException")
        .WithApiVersionSet(app.GetApiVersionSet())
        .MapToApiVersion(new ApiVersion(1, 0))
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Test Exception.",
            Description = "Test Exception.",
            Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Test Exception" } }
        });









    } 
}