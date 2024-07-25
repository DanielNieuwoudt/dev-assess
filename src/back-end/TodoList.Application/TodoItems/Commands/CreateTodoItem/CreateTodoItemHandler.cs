using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record CreateTodoItemResponse(TodoItem TodoItem);

    public sealed class CreateTodoItemHandler(ITodoItemsRepository repository, ILogger<CreateTodoItemHandler> logger)
        : IRequestHandler<CreateTodoItemCommand, Result<ApplicationError, CreateTodoItemResponse>>
    {
        private readonly ITodoItemsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<CreateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<Result<ApplicationError, CreateTodoItemResponse>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding duplicate todo items based on id.");

            if ( await _repository.FindByIdAsync(new TodoItemId(request.Id), cancellationToken))
            {
                return new DuplicateError(new Dictionary<string, string[]>
                {
                    { nameof(request.Id), new[] { request.Id.ToString() } }
                });
            }

            _logger.LogInformation("Finding duplicate todo items based on description.");

            if ( await _repository.FindByDescriptionAsync(request.Description.Trim(), cancellationToken))
            {
                return new DuplicateError(new Dictionary<string, string[]>
                {
                    { nameof(request.Description), new[] { request.Description.Trim() } }
                });
            }

            _logger.LogInformation("Creating todo item.");

            var todoItemId = new TodoItemId(request.Id);
            var todoItemToCreate = new TodoItem(todoItemId, 
                request.Description, 
                request.isCompleted, 
                DateTimeOffset.Now, 
                DateTimeOffset.Now);
            
            var createdTodoItem = await _repository
                .CreateTodoItemAsync(todoItemToCreate, cancellationToken);

            _logger.LogInformation("Todo item created.");

            return new CreateTodoItemResponse(createdTodoItem);
        }
    }
}
