using Asp.Versioning;
using MediatR;
using Microservice.Customer.Address.Api.Extensions;
using Microservice.Customer.Address.Api.Helpers.Exceptions;
using Microservice.Customer.Address.Api.Helpers.Interfaces;
using Microservice.Customer.Address.Api.MediatR.AddCustomerAddress;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddress;
using Microservice.Customer.Address.Api.MediatR.GetCustomerAddresses;
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
        var customerGroup = app.MapGroup("api/v{version:apiVersion}/customer-address").WithTags("customerAddress");

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

       customerGroup.MapGet("", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async ([FromServices] IMediator mediator, ICustomerAddressHttpAccessor customerAddressHttpAccessor) =>
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
       .WithName("AddCustomer")
       .WithApiVersionSet(app.GetApiVersionSet())
       .MapToApiVersion(new ApiVersion(1, 0))
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "Add a customer address.",
           Description = "Adds a customer address.",
           Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers Address" } }
       }); 





        





        //app.MapGet("api/v{version:apiVersion}/customer/{id}", async ([FromRoute] Guid id, [FromServices] IMediator mediator) =>
        //{
        //    var getCustomerResponse = await mediator.Send(new GetCustomerRequest(id));
        //    return Results.Ok(getCustomerResponse);
        //})        
        //.Produces<GetCustomerResponse>((int)HttpStatusCode.OK)
        //.Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        //.WithName("GetCustomer")
        //.WithApiVersionSet(app.GetApiVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        ////.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Get a customer based on id.",
        //    Description = "Gets a customer based on its id.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers" } }
        //});

        //app.MapGet("api/v{version:apiVersion}/book/title/{criteria}", async ([FromRoute] string criteria, [FromServices] IMediator mediator) => {
        //    var searchCustomerTitleResponse = await mediator.Send(new SearchCustomerTitleRequest(criteria));
        //    return Results.Ok(searchCustomerTitleResponse);
        //})
        //.Accepts<SearchCustomerTitleRequest>("application/json")
        //.Produces<SearchCustomerTitleResponse>((int)HttpStatusCode.OK)
        //.Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        //.WithName("SearchCustomerTitle")
        //.WithApiVersionSet(app.GetApiVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        //.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Search for books based on title.",
        //    Description = "Gets books based on title.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers" } }
        //});

        //app.MapPost("api/v{version:apiVersion}/book/add", async (AddCustomerRequest addCustomerRequest, IMediator mediator) => {
        //    var addCustomerResponse = await mediator.Send(addCustomerRequest);
        //    return Results.Ok(addCustomerResponse);
        //})
        //.Accepts<AddCustomerRequest>("application/json")
        //.Produces<AddCustomerResponse>((int)HttpStatusCode.OK)
        //.Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        //.WithName("AddCustomer")
        //.WithApiVersionSet(app.GetApiVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        //.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Add a book.",
        //    Description = "Adds a book.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers" } }
        //});

        //app.MapPut("api/v{version:apiVersion}/book/update", async (UpdateCustomerRequest updateCustomerRequest, IMediator mediator) => {
        //    var updateCustomerResponse = await mediator.Send(updateCustomerRequest);
        //    return Results.Ok(updateCustomerResponse);
        //})
        //.Accepts<UpdateCustomerRequest>("application/json")
        //.Produces<UpdateCustomerResponse>((int)HttpStatusCode.OK)
        //.Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        //.WithName("UpdateCustomer")
        //.WithApiVersionSet(app.GetApiVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        //.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Update a book.",
        //    Description = "Updates a book.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers" } }
        //});

        //app.MapDelete("api/v{version:apiVersion}/book/{id}", async ([FromRoute] Guid id, [FromServices] IMediator mediator) => {
        //    var deleteCustomerResponse = await mediator.Send(new DeleteCustomerRequest(id));
        //    return Results.Ok(deleteCustomerResponse);
        //})
        //.Accepts<DeleteCustomerRequest>("application/json")
        //.Produces((int)HttpStatusCode.OK)
        //.Produces<BadRequestException>((int)HttpStatusCode.BadRequest)
        //.WithName("DeleteCustomer")
        //.WithApiVersionSet(app.GetApiVersionSet())
        //.MapToApiVersion(new ApiVersion(1, 0))
        //.RequireAuthorization()
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Delete a book.",
        //    Description = "Deletes a book.",
        //    Tags = new List<OpenApiTag> { new() { Name = "Microservice Customer System - Customers" } }
        //});
    } 
}