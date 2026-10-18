using DoctorPilot.Application.Exceptions;
using DoctorPilot.Contracts.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace DoctorPilot.Api.ExceptionHandling;

public sealed class ApplicationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ForbiddenException => StatusCodes.Status403Forbidden,
            ConflictException => StatusCodes.Status409Conflict,
            _ => (int?)null
        };

        if (statusCode is null)
            return false;

        httpContext.Response.StatusCode = statusCode.Value;
        await httpContext.Response.WriteAsJsonAsync(
            ResultViewModel<object?>.Error(exception.Message),
            cancellationToken);

        return true;
    }
}
