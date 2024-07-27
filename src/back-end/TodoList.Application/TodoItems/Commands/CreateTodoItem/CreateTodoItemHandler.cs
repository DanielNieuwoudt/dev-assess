using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record CreateTodoItemResponse(TodoItem TodoItem);

    public sealed class CreateTodoItemHandler(ITodoItemsReadRepository readRepository, ITodoItemsWriteRepository writeRepository, ILogger<CreateTodoItemHandler> logger)
        : IRequestHandler<CreateTodoItemCommand, TodoItemResult<ApplicationError, CreateTodoItemResponse>>
    {
        private readonly ITodoItemsReadRepository _readRepository = readRepository ?? throw new ArgumentNullException(nameof(readRepository));
        private readonly ITodoItemsWriteRepository _writeRepository = writeRepository ?? throw new ArgumentNullException(nameof(writeRepository));
        private readonly ILogger<CreateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItemResult<ApplicationError, CreateTodoItemResponse>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding duplicate todo items based on Id: {Id}", request.Id);

            if (await _readRepository.FindByIdAsync(new TodoItemId(request.Id), cancellationToken))
            {
                _logger.LogWarning("Todo item already exists with Id: {Id}", request.Id);

                return new DuplicateError(nameof(request.Id), request.Id.ToString());
            }

            _logger.LogInformation("Finding duplicate todo items based on Description: {Description}", request.Description.Trim());

            if ( await _readRepository.FindByDescriptionAsync(request.Description.Trim(), cancellationToken))
            {
                _logger.LogWarning("Todo item already exists with Description: {Description}", request.Description.Trim());

                return new DuplicateError(nameof(request.Description), request.Description.Trim());
            }

            var todoItemId = new TodoItemId(request.Id);
            var todoItemToCreate = new TodoItem(todoItemId, 
                request.Description, 
                request.isCompleted, 
                DateTimeOffset.Now, 
                DateTimeOffset.Now);

            _logger.LogInformation("Creating todo item with Id: {Id}", todoItemId.Value);

            var createdTodoItem = await _writeRepository
                .CreateTodoItemAsync(todoItemToCreate, cancellationToken);

            _logger.LogInformation("Todo item created with Id: {Id}", createdTodoItem.Id.Value);

            return new CreateTodoItemResponse(createdTodoItem);
        }
    }
}
