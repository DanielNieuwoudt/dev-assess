using FluentValidation.Results;

namespace TodoList.Application.TodoItems.Extensions
{
    public static class ValidationFailureExtensions
    {
        public static IDictionary<string, string[]> ToErrorDictionary(this List<ValidationFailure>? validationFailures)
        {
            if (validationFailures == null)
                return new Dictionary<string, string[]>();

            return validationFailures
                .GroupBy(failure => failure.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => failure.ErrorMessage).ToArray());
        }
    }
}
