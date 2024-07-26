using System.Diagnostics.CodeAnalysis;
using TodoList.Application.TodoItems.Enumerations;

namespace TodoList.Application.TodoItems.Errors;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record NotFoundError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.NotFound, errors);