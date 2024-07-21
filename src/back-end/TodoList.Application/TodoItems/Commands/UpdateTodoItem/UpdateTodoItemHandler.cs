using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem
{
    public sealed record UpdateTodoItemResult;

    public sealed class UpdateTodoItemHandler(ITodoItemsRepository repository, ILogger<UpdateTodoItemHandler> logger)
        : IRequestHandler<UpdateTodoItemCommand, UpdateTodoItemResult>
    {
        private readonly ITodoItemsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<UpdateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<UpdateTodoItemResult> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo item.");
            var todoItem = await _repository.GetTodoItemAsync(new TodoItemId(request.Id), cancellationToken);
            if (todoItem is null)
            {
                throw new TodoItemNotFoundException(nameof(TodoItem), request.Id);
            }

            _logger.LogInformation("Updating todo item.");
            if (request.IsCompleted)
                todoItem.MarkAsCompleted();
            else
                todoItem.MarkAsInCompleted();

            todoItem.SetDescription(request.Description);
            todoItem.SetModified();
            
            await _repository.UpdateTodoItemAsync(todoItem, cancellationToken);

            _logger.LogInformation("Todo item updated.");

            return new UpdateTodoItemResult();
        }
    }
}
