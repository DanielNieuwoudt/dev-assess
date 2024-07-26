using TodoList.Application.Common.Errors;

namespace TodoList.Application.TodoItems
{
    public class TodoItemResult<TError, TValue> where TError : ApplicationError
    {
        private readonly ApplicationError? _error;
        
        private readonly object? _value;
        
        private bool _isError => _error is not null;

        public TValue? Value => _value is TValue value ? value : default;
        public TError? Error => _error as TError;
        public bool IsError => _isError;

        public TodoItemResult(TValue value)
        {
            _value = value;
        }

        public TodoItemResult(TError error)
        {
            _error = error;
        }
       
        /// <summary>
        /// Allows us to create a TodoItemResult implicitly from a TValue not having to declare the return type.
        /// </summary>
        public static implicit operator TodoItemResult<TError, TValue>(TValue value)
        {
            return new TodoItemResult<TError, TValue>(value);
        }

        /// <summary>
        /// Allows us to create a TodoItemResult implicitly from a TError not having to declare the return type.
        /// </summary>
        public static implicit operator TodoItemResult<TError, TValue>(TError error)
        {
            return new TodoItemResult<TError, TValue>(error);
        }
    }
}
