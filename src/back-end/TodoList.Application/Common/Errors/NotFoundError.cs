using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors;

public sealed record NotFoundError(IDictionary<string, string[]> errors) : ApplicationError(ErrorReason.NotFound, errors);