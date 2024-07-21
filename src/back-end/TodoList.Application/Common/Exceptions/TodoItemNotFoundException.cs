using System.Diagnostics.CodeAnalysis;

namespace TodoList.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Exception")]
    public sealed class TodoItemNotFoundException : Exception
    {
        public TodoItemNotFoundException()
        {
        }

        public TodoItemNotFoundException(string message) : base(message)
        {
        }

        public TodoItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TodoItemNotFoundException(string name, object key) : base($"Entity \"{name}\" with ({key}) was not found.")
        {
        }
    }
}
