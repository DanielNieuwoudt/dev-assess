using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems
{
    public sealed record GetTodoItemsResponse (IEnumerable<TodoItem> TodoItems);

    public sealed class GetTodoItemsHandler(ITodoItemsReadRepository readRepository, ILogger<GetTodoItemsHandler> logger) : IRequestHandler<GetTodoItemsQuery, TodoItemResult<ApplicationError, GetTodoItemsResponse>>
    {
        private readonly ITodoItemsReadRepository _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        private readonly ILogger<GetTodoItemsHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, GetTodoItemsResponse>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo items.");

            var todoItems = await _readRepository.GetTodoItemsAsync(cancellationToken);

            _logger.LogInformation("Returning todo items.");

            return new GetTodoItemsResponse(todoItems.ToList());
        }
    }
}
