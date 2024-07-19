using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems
{
    public sealed record GetTodoItemsResult (IEnumerable<TodoItem> TodoItems);

    public sealed class GetTodoItemsHandler(ITodoItemsRepository repository, ILogger<GetTodoItemsHandler> logger) : IRequestHandler<GetTodoItemsQuery, GetTodoItemsResult>
    {
        private readonly ITodoItemsRepository _repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        private readonly ILogger<GetTodoItemsHandler> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<GetTodoItemsResult> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo items.");

            var todoItems = await _repository.GetTodoItemsAsync(cancellationToken);

            _logger.LogInformation("Returning todo items.");

            return new GetTodoItemsResult(todoItems.ToList());
        }
    }
}
