using FluentValidation;
using MediatR;
using TodoList.Application.TodoItems.Extensions;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.Errors;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItems;

namespace TodoList.Application.TodoItems.Behaviours
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
                if (responseType == typeof(TodoItemResult<ApplicationError, CreateTodoItemResponse>))
                {
                    return (TResponse)(object)new TodoItemResult<ApplicationError, CreateTodoItemResponse>(validationError);
                }
                if (responseType == typeof(TodoItemResult<ApplicationError, UpdateTodoItemResponse>))
                {
                    return (TResponse)(object)new TodoItemResult<ApplicationError, UpdateTodoItemResponse>(validationError);
                }
                if (responseType == typeof(TodoItemResult<ApplicationError, GetTodoItemResponse>))
                {
                    return (TResponse)(object)new TodoItemResult<ApplicationError, GetTodoItemResponse>(validationError);
                }
                if (responseType == typeof(TodoItemResult<ApplicationError, GetTodoItemsResponse>))
                {
                    return (TResponse)(object)new TodoItemResult<ApplicationError, GetTodoItemsResponse>(validationError);
                }

                throw new InvalidOperationException($"Unhandled response type: {responseType.Name}");
                
            }

            return await next();
        }
    }
}