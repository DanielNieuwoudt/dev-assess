using TodoList.Application.TodoItems.Errors;

namespace TodoList.Application.TodoItems
{
    public class TodoItemResult<TError, TValue> where TError : ApplicationError
    {
        private readonly ApplicationError? _error;
        private readonly object? _value;
        private bool _isError => _error is not null;

        public TodoItemResult(TValue value)
        {
            _value = value;
        }

        public TodoItemResult(TError error)
        {
            _error = error;
        }

        public TValue? Value => _value is TValue value ? value : default;
        public TError? Error => _error as TError;
        public bool IsError => _isError;

        public static implicit operator TodoItemResult<TError, TValue>(TValue value)
        {
            return new TodoItemResult<TError, TValue>(value);
        }

        public static implicit operator TodoItemResult<TError, TValue>(TError error)
        {
            return new TodoItemResult<TError, TValue>(error);
        }
    }
}
