using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed class ValidationError : ApplicationError
{
    public ValidationError(string property, string message) : base(ErrorReason.Validation, property, message)
    {
    }

    public ValidationError(IDictionary<string, string[]> errors) : base(ErrorReason.Validation, errors)
    {
    }
}