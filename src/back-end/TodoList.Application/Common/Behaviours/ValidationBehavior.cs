using FluentValidation;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common.Extensions;
using TodoList.Application.TodoItems;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItems;

namespace TodoList.Application.Common.Behaviours
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var failures = validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                var validationError = new ValidationError(failures.ToErrorDictionary());

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(TodoItemResult<,>))
                {
                    return CreateTodoItemResult(validationError, responseType);
                }

                throw new InvalidOperationException($"Unhandled response type: {responseType.Name}");
                
            }

            return await next();
        }
        
        private TResponse CreateTodoItemResult(ValidationError validationError, Type responseType)
        {
            var genericArguments = responseType.GetGenericArguments();
            var todoItemResultType = typeof(TodoItemResult<,>).MakeGenericType(genericArguments);
            return (TResponse)Activator.CreateInstance(todoItemResultType, validationError)!;
        }
    }
}