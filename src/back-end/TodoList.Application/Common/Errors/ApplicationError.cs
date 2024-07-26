using System.Diagnostics.CodeAnalysis;
using TodoList.Application.Common.Enumerations;

namespace TodoList.Application.Common.Errors
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public abstract record ApplicationError(ErrorReason Reason, IDictionary<string, string[]> errors);
}
