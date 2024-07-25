using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

public sealed record ValidationError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.Validation, errors);