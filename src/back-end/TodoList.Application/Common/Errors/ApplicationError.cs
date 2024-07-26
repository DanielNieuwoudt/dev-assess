using System.Diagnostics.CodeAnalysis;
using TodoList.Application.TodoItems.Enumerations;

namespace TodoList.Application.TodoItems.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public abstract record ApplicationError(ErrorReason Reason, IDictionary<string, string[]> errors);
}
