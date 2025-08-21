using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace AirAstana.FlightControl.Application.Common.Exceptions;

public class RequestValidationException: ValidationException
{
    /// <inheritdoc />
    public RequestValidationException() : base("Ошибка во время валидации тела запроса")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <inheritdoc />
    public RequestValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
#pragma warning disable CS0108, CS0114
    public IDictionary<string, string[]> Errors { get; }
#pragma warning restore CS0108, CS0114
}