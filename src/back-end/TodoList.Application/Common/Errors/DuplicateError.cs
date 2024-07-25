using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

public sealed record DuplicateError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.Duplicate, errors);