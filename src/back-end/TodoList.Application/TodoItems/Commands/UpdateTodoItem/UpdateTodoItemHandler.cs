using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem
{
    public sealed record UpdateTodoItemResponse;

    public sealed class UpdateTodoItemHandler(ITodoItemsReadRepository readRepository, ITodoItemsWriteRepository writeRepository, ILogger<UpdateTodoItemHandler> logger)
        : IRequestHandler<UpdateTodoItemCommand, TodoItemResult<ApplicationError, UpdateTodoItemResponse>>
    {
        private readonly ITodoItemsReadRepository _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        private readonly ITodoItemsWriteRepository _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        private readonly ILogger<UpdateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, UpdateTodoItemResponse>> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting todo item with Id: {Id}", request.Id);
            var todoItem = await _readRepository.GetTodoItemAsync(new TodoItemId(request.Id), cancellationToken);
            if (todoItem is null)
            {
                return new NotFoundError(new Dictionary<string, string[]> 
                {
                    { nameof(request.Id), new [] { request.Id.ToString() } }
                });
            }

            _logger.LogInformation("Updating todo item with Id: {Id}", request.Id);
            if (request.IsCompleted)
                todoItem.MarkAsCompleted();
            else
                todoItem.MarkAsInCompleted();

            todoItem.SetDescription(request.Description);
            todoItem.SetModified();
            
            await _writeRepository.UpdateTodoItemAsync(todoItem, cancellationToken);

            _logger.LogInformation("Todo item updated with Id: {Id}", todoItem.Id.Value);

            return new UpdateTodoItemResponse();
        }
    }
}
