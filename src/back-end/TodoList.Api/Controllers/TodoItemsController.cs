using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.GetTodoItems;
using TodoList.Application.TodoItems.Queries.GetTodoItem;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController(IMapper mapper, ISender sender, ILogger<TodoItemsController> logger)
        : ControllerBase
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        private readonly ILogger<TodoItemsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet]
        public async Task<IActionResult> GetTodoItems(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all todo items");

            var results = await _sender.Send(new GetTodoItemsQuery(), cancellationToken);
            var todoItems = _mapper.Map<IEnumerable<Generated.TodoItem>>(results.TodoItems);

            return Ok(todoItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id, CancellationToken cancellation)
        {
            _logger.LogInformation("Getting todo item");

            var result = await _sender
                .Send(new GetTodoItemQuery(id), cancellation);

            var todoItem = _mapper
                .Map<Generated.TodoItem>(result.TodoItem);

            return Ok(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            _logger.LogInformation("Updating todo item");

            await _sender.Send(new UpdateTodoItemCommand(id, todoItem.Id, todoItem.Description, todoItem.IsCompleted));

            return NoContent();
        } 

        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            _logger.LogInformation("Creating todo item");

            var result = await _sender
                .Send(new CreateTodoItemCommand(todoItem.Id, todoItem.Description, todoItem.IsCompleted));

            var createdTodoItem = _mapper
                .Map<Generated.TodoItem>(result.TodoItem);
             
            return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
        } 
    }
}
