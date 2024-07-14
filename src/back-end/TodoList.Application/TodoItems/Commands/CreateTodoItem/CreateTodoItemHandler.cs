using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.Logging;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record CreateTodoItemResult(TodoItem TodoItem);

    public sealed class CreateTodoItemHandler(ITodoItemsRepository repository, ILogger<CreateTodoItemHandler> logger)
        : IRequestHandler<CreateTodoItemCommand, CreateTodoItemResult>
    {
        private readonly ITodoItemsRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ILogger<CreateTodoItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CreateTodoItemResult> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding duplicate todo items based in id.");

            if ( await _repository.FindDuplicateTodoItemAsync(ti => ti.Id == new TodoItemId(request.Id) && 
                                                               ti.IsCompleted == false, cancellationToken))
            {
                throw new TodoItemDuplicateException(nameof(request.Id), request.Id);
            }

            _logger.LogInformation("Finding duplicate todo items based in description.");

            if ( await _repository.FindDuplicateTodoItemAsync(ti => ti.Description == request.Description && 
                                                               ti.IsCompleted == false, cancellationToken))
            {
                throw new TodoItemDuplicateException(nameof(request.Description), request.Description);
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

            return new CreateTodoItemResult(createdTodoItem);
        }
    }
}
