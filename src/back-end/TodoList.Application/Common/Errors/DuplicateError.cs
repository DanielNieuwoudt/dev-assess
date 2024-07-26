using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record DuplicateError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.Duplicate, errors);