using FluentValidation;
using FluentValidation.Results;
using Mediator;

namespace DoctorPilot.Application.Behaviors;

public sealed class ValidationBehavior<TMessage, TResponse>
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    private readonly IReadOnlyCollection<IValidator<TMessage>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TMessage>> validators)
    {
        _validators = validators.ToArray();
    }

    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Count == 0)
            return await next(message, cancellationToken);

        var context = new ValidationContext<TMessage>(message);
        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .DistinctBy(FailureKey)
            .OrderBy(failure => failure.PropertyName, StringComparer.Ordinal)
            .ThenBy(failure => failure.ErrorMessage, StringComparer.Ordinal)
            .ThenBy(failure => failure.ErrorCode, StringComparer.Ordinal)
            .ThenBy(failure => failure.Severity)
            .ToArray();

        if (failures.Length != 0)
            throw new ValidationException(failures);

        return await next(message, cancellationToken);
    }

    private static (string PropertyName, string ErrorMessage, string ErrorCode, Severity Severity) FailureKey(
        ValidationFailure failure)
        => (failure.PropertyName, failure.ErrorMessage, failure.ErrorCode, failure.Severity);
}
