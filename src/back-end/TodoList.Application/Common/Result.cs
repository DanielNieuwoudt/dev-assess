using TodoList.Application.Common.Errors;

namespace TodoList.Application.Common
{
    public class Result<TError, TValue> where TError : ApplicationError
    {
        private readonly ApplicationError? _error;
        private readonly object? _value;
        private bool _isError => _error is not null;

        public Result(TValue value)
        {
            _value = value;
        }

        public Result(TError error)
        {
            _error = error;
        }

        public TValue? Value => _value is TValue value ? value : default;
        public TError? Error => _error as TError;
        public bool IsError => _isError;

        public static implicit operator Result<TError, TValue>(TValue value)
        {
            return new Result<TError, TValue>(value);
        }

        public static implicit operator Result<TError, TValue>(TError error)
        {
            return new Result<TError, TValue>(error);
        }
    }
}
