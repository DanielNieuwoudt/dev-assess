using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Queries.GetTodoItem
{
    public sealed record GetTodoItemResponse(TodoItem? TodoItem);

    public class GetTodoItemHandler(ITodoItemsReadRepository readRepository, ILogger<GetTodoItemHandler> logger)
        : IRequestHandler<GetTodoItemQuery, TodoItemResult<ApplicationError, GetTodoItemResponse>>
    {
        private readonly ITodoItemsReadRepository _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        private readonly ILogger<GetTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, GetTodoItemResponse>> Handle(GetTodoItemQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo item with Id: {Id}", request.Id);

            var todoItem = await _readRepository.GetTodoItemAsync(new TodoItemId(request.Id), cancellationToken);
            if (todoItem == null)
            {
                _logger.LogWarning("Todo item was not found with Id: {Id}", request.Id);

                return new NotFoundError(nameof(request.Id), request.Id.ToString());
            }

            _logger.LogInformation("Returning todo item with Id: {Id}", todoItem.Id.Value);

            return new GetTodoItemResponse(todoItem);
        }
    }
}
