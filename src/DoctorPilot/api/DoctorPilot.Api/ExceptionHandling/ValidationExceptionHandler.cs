using DoctorPilot.Contracts.Common;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace DoctorPilot.Api.ExceptionHandling;

public sealed class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
            return false;

        var messages = validationException.Errors
            .Where(failure => failure is not null && !string.IsNullOrWhiteSpace(failure.ErrorMessage))
            .OrderBy(failure => failure.PropertyName, StringComparer.Ordinal)
            .ThenBy(failure => failure.ErrorMessage, StringComparer.Ordinal)
            .Select(failure => failure.ErrorMessage)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var message = messages.Length == 0
            ? "Falha de validação."
            : string.Join(" ", messages);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        await httpContext.Response.WriteAsJsonAsync(
            ResultViewModel<object?>.Error(message),
            cancellationToken);

        return true;
    }
}
