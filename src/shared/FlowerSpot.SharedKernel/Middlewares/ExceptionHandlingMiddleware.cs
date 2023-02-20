using FlowerSpot.SharedKernel.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;
using System.Net;
using FluentValidation;

namespace FlowerSpot.SharedKernel.Middlewares;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionHandlingMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch(Exception exception)
        {
            await HandleExceptionAsync(context, exception, logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionHandlingMiddleware> logger)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var message = new StringBuilder();

        switch (exception)
        {
            case ValidationException:
                var ex = exception as ValidationException;
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                message.Append(string.Join(" ", ex.Errors.Select(e => e.ErrorMessage).Distinct()));

                logger.LogWarning(exception, message.ToString());

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage = message.ToString() }), Encoding.UTF8);
                return;
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage = exception.Message }), Encoding.UTF8);
                logger.LogError(exception, exception.Message);
                return;
            case UnauthorizedException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage = exception.Message }), Encoding.UTF8);
                logger.LogError(exception, exception.Message);
                return;
            case ConfigurationException:
            default:
                logger.LogError(exception, exception.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        message.Append($"An error occurred. Please try again.");

        logger.LogCritical(exception, message.ToString());

        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { errorMessage = message.ToString() }), Encoding.UTF8);
    }
}
