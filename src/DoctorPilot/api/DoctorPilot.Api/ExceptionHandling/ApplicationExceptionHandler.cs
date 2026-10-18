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
        if (exception is not NotFoundException notFoundException)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        await httpContext.Response.WriteAsJsonAsync(
            ResultViewModel<object?>.Error(notFoundException.Message),
            cancellationToken);

        return true;
    }
}
