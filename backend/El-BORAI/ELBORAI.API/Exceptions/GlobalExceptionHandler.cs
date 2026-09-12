using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ELBORAI.API.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var problemDetails = new ProblemDetails
        {
            Status = exception switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            },

            Title = exception switch
            {
                UnauthorizedAccessException =>
                    "Unauthorized",

                _ =>
                    "An unexpected error occurred."
            },

            Detail = exception switch
            {
                UnauthorizedAccessException =>
                    "Authentication is required to perform this operation.",

                _ =>
                    "An unexpected error occurred while processing the request."
            },

            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode =
            problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}