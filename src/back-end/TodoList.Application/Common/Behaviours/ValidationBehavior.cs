using FluentValidation;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common.Extensions;
using TodoList.Application.TodoItems;

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
                var genericArguments = typeof(TResponse).GetGenericArguments();
                var todoItemResultType = typeof(TodoItemResult<,>).MakeGenericType(genericArguments);
                
                return (TResponse)Activator.CreateInstance(todoItemResultType, validationError)!;
            }

            return await next();
        }
    }
}