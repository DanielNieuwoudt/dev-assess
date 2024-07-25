using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors
{
    public record ApplicationError(ErrorReason Reason, IDictionary<string, string[]> errors);
}
