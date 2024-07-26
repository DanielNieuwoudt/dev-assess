using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record NotFoundError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.NotFound, errors);