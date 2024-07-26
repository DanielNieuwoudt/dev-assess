using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.TodoItems.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem
{
    public sealed record UpdateTodoItemResponse;

    public sealed class UpdateTodoItemHandler(ITodoItemsRepository repository, ILogger<UpdateTodoItemHandler> logger)
        : IRequestHandler<UpdateTodoItemCommand, TodoItemResult<ApplicationError, UpdateTodoItemResponse>>
    {
        private readonly ITodoItemsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<UpdateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, UpdateTodoItemResponse>> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo item.");
            var todoItem = await _repository.GetTodoItemAsync(new TodoItemId(request.Id), cancellationToken);
            if (todoItem is null)
            {
                return new NotFoundError(new Dictionary<string, string[]> 
                {
                    { nameof(request.Id), new [] { request.Id.ToString() } }
                });
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

            return new UpdateTodoItemResponse();
        }
    }
}
