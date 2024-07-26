using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Queries.GetTodoItem
{
    public sealed record GetTodoItemResponse(TodoItem? TodoItem);

    public class GetTodoItemHandler(ITodoItemsRepository repository, ILogger<GetTodoItemHandler> logger)
        : IRequestHandler<GetTodoItemQuery, TodoItemResult<ApplicationError, GetTodoItemResponse>>
    {
        private readonly ITodoItemsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<GetTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, GetTodoItemResponse>> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo item with id {Id}", request.Id);

            var todoItem = await _repository.GetTodoItemAsync(new TodoItemId(request.Id), cancellationToken);

            if (todoItem == null)
            {
                return new NotFoundError(new Dictionary<string, string[]>
                {
                    { nameof(request.Id), new[] { request.Id.ToString() } }
                });
            }

            _logger.LogInformation("Returning todo item with id {Id}", todoItem.Id.Value);

            return new GetTodoItemResponse(todoItem);
        }
    }
}
