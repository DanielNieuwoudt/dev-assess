using System.Diagnostics.CodeAnalysis;

namespace TodoList.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Exception")]
    public sealed class TodoItemInvalidException : Exception
    {
        public TodoItemInvalidException() { }
        public TodoItemInvalidException(string message) : base(message) { }
        public TodoItemInvalidException(string message, Exception innerException) : base(message, innerException)
        { }

        public TodoItemInvalidException(object requestId, object key)
            : base($"The request id \"{requestId}\" did not match the todo item ({key}).")
        {
        }
    }
}
