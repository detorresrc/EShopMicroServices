using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SharedKernel.Exceptions.Handler;

public sealed class CustomExceptionHandler(
    ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage}, Time of occurence {time}",
            exception.Message,
            DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            BadRequestException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            ValidationException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity
            ),
            NotFoundException =>
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ =>
            (
                "An unexpected error occurred.",
                "InternalServerError",
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };

        var problemDetails = new CustomErrorDetails
        {
            Title = details.Title,
            Detail = details.Detail,
            StatusCode = details.StatusCode,
            Instance = context.Request.Path
        };

        problemDetails.TraceId = context.TraceIdentifier;
        if (exception is ValidationException validationException)
            problemDetails.Errors =
                validationException
                    .Errors
                    .Select(e => new ValidationError{ PropertyName = e.PropertyName, ErrorMessage = e.ErrorMessage });

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}