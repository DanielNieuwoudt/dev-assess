using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TodoList.Api.Common.ExceptionFilters;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.GetTodoItems;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Api.Generated;

namespace TodoList.Api.Controllers
{
    [ApiController]
    public class TodoItemsController(IMapper mapper, ISender sender, ILogger<TodoItemsController> logger) : TodoItemsControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        private readonly ILogger<TodoItemsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override async Task<ActionResult<ICollection<TodoItem>>> GetTodoItems(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Getting all todo items");

            var results = await _sender
                .Send(new GetTodoItemsQuery(), cancellationToken);

            var todoItems = _mapper
                .Map<IEnumerable<TodoItem>>(results.TodoItems);

            _logger.LogInformation("Returning all todo items");

            return Ok(todoItems);
        }

        [TodoItemValidationExceptionFilter]
        [TodoItemNotFoundExceptionFilter]
        public override async Task<ActionResult<TodoItem>> GetTodoItem(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Getting todo item");

            var result = await _sender
                .Send(new GetTodoItemQuery(id), cancellationToken);

            var todoItem = _mapper
                .Map<TodoItem>(result.TodoItem);

            _logger.LogInformation("Returning todo item");

            return Ok(todoItem);
        }

        [TodoItemValidationExceptionFilter]
        [TodoItemDuplicateExceptionFilter]
        [TodoItemNotFoundExceptionFilter]
        public override async Task<IActionResult> PutTodoItem(Guid id, TodoItem body, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Validating todo item");

            if (id != body.Id)
                return BadRequest();

            _logger.LogInformation("Updating todo item");

            await _sender
                .Send(new UpdateTodoItemCommand(body.Id, body.Description, body.IsCompleted), cancellationToken);

            _logger.LogInformation("Todo item updated");

            return NoContent();          
        }

        [TodoItemValidationExceptionFilter]
        [TodoItemDuplicateExceptionFilter]
        public override async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem body, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Creating todo item");

            var result = await _sender
                .Send(new CreateTodoItemCommand(body.Id, body.Description, body.IsCompleted), cancellationToken);

            var createdTodoItem = _mapper
                .Map<TodoItem>(result.TodoItem);
       
            _logger.LogInformation("Todo item created");

            return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
        }
    }
}
