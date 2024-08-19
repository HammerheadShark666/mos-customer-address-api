using FluentValidation;
using FluentValidation.Results;
using Microservice.Customer.Address.Api.Helpers.Exceptions; 
using System.Net;
using System.Text.Json;
using static Microservice.Customer.Address.Api.Helpers.Enums;

namespace Microservice.Customer.Address.Api.Middleware;

internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var httpStatusCode = HttpStatusCode.InternalServerError;

        context.Response.ContentType = "application/json";

        var result = string.Empty;

        switch (exception)
        {
            case ValidationException validationException:
                httpStatusCode = HttpStatusCode.BadRequest; 
                result = JsonSerializer.Serialize(GetValidationErrors(validationException.Errors)); 
                break;
            case ArgumentException argumentException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(argumentException.Message);
                break;
            case BadRequestException badRequestException:
                httpStatusCode = HttpStatusCode.BadRequest;
                result = badRequestException.Message;
                break;
            case NotFoundException:
                httpStatusCode = HttpStatusCode.NotFound;
                break;
            case not null:
                httpStatusCode = HttpStatusCode.BadRequest;
                break;
        }

        context.Response.StatusCode = (int)httpStatusCode;

        if (result == string.Empty) result = JsonSerializer.Serialize(new { error = exception?.Message });

        return context.Response.WriteAsync(result);
    } 

    private static IEnumerable<Helpers.ValidationError> GetValidationErrors(IEnumerable<ValidationFailure> validationErrors)
    {
        if (validationErrors != null)
        {
            foreach (var error in validationErrors)
            {
                yield return new Helpers.ValidationError(ErrorType.Error.ToString(), error.ErrorMessage);
            }
        }
    }



    //private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    //{
    //    var statusCode = GetStatusCode(exception);

    //    httpContext.Response.ContentType = "application/json";
    //    httpContext.Response.StatusCode = statusCode;

    //    switch (statusCode)
    //    {
    //        case 404:
    //            {
    //                var response = new
    //                {
    //                    status = statusCode,
    //                    detail = GetMessage(exception)
    //                };

    //                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    //                break;
    //            }
    //        default:
    //            {
    //                var response = new
    //                {
    //                    status = statusCode,
    //                    detail = GetMessage(exception),
    //                    errors = GetErrors(exception)
    //                };

    //                await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    //                break;
    //            }
    //    }
    //}

    //private static int GetStatusCode(Exception exception) =>
    //    exception switch
    //    {
    //        BadRequestException => StatusCodes.Status400BadRequest,
    //        NotFoundException => StatusCodes.Status404NotFound,
    //        ValidationException => StatusCodes.Status400BadRequest,
    //        _ => StatusCodes.Status500InternalServerError
    //    };

    //private static string GetMessage(Exception exception) =>
    //    exception switch
    //    {
    //        ValidationException => "Validation Error",
    //        _ => exception.Message
    //    };

    //private static IEnumerable<string> GetErrors(Exception exception)
    //{
    //    if (exception is ValidationException validationException)
    //    {
    //        foreach (var error in validationException.Errors)
    //        {
    //            yield return error.ErrorMessage;
    //        }
    //    }
    //}
}

//internal sealed class ExceptionHandlingMiddleware : IMiddleware
//{
//    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

//    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

//    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception e)
//        {
//            _logger.LogError(e, e.Message);
//            await HandleExceptionAsync(context, e);
//        }
//    }

//    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
//    {
//        var statusCode = GetStatusCode(exception);

//        var response = new
//        { 
//            status = statusCode,
//            detail = GetMessage(exception),
//            errors = GetErrors(exception)
//        };

//        httpContext.Response.ContentType = "application/json";
//        httpContext.Response.StatusCode = statusCode;
//        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
//    }

//    private static int GetStatusCode(Exception exception) =>
//        exception switch
//        {
//            BadRequestException => StatusCodes.Status400BadRequest,
//            NotFoundException => StatusCodes.Status404NotFound,
//            ValidationException => StatusCodes.Status400BadRequest,
//            _ => StatusCodes.Status500InternalServerError
//        }; 

//    private static string GetMessage(Exception exception) =>
//        exception switch
//        {
//            ValidationException => "Validation Error",
//            _ => exception.Message
//        };  

//    private static IEnumerable<string> GetErrors(Exception exception)
//    { 
//        if (exception is ValidationException validationException)
//        {  
//            foreach (var  error in validationException.Errors)
//            {
//                yield return error.ErrorMessage;
//            }
//        }
//    }
//}