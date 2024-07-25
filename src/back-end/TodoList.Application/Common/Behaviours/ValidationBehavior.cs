using FluentValidation;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common.Extensions;
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
                if (responseType == typeof(Result<ApplicationError, CreateTodoItemResponse>))
                {
                    return (TResponse)(object)new Result<ApplicationError, CreateTodoItemResponse>(validationError);
                }
                if (responseType == typeof(Result<ApplicationError, UpdateTodoItemResponse>))
                {
                    return (TResponse)(object)new Result<ApplicationError, UpdateTodoItemResponse>(validationError);
                }
                if (responseType == typeof(Result<ApplicationError, GetTodoItemResponse>))
                {
                    return (TResponse)(object)new Result<ApplicationError, GetTodoItemResponse>(validationError);
                }
                if (responseType == typeof(Result<ApplicationError, GetTodoItemsResponse>))
                {
                    return (TResponse)(object)new Result<ApplicationError, GetTodoItemsResponse>(validationError);
                }

                throw new InvalidOperationException($"Unhandled response type: {responseType.Name}");
                
            }

            return await next();
        }
    }
}