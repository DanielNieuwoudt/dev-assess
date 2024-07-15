using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using FluentValidation.Results;

namespace TodoList.Application.Common.Exceptions
{
    [ExcludeFromCodeCoverage(Justification = "Exception")]
    public sealed class TodoItemDuplicateException : ValidationException
    {
        public TodoItemDuplicateException(string message) : base(message)
        {
        }

        public TodoItemDuplicateException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
        {
        }

        public TodoItemDuplicateException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
        {
        }

        public TodoItemDuplicateException(IEnumerable<ValidationFailure> errors) : base(errors)
        {
        }
            
    }
}
