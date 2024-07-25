using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using TodoList.Api.Common.Constants;
using TodoList.Api.Common.Helpers;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Api.Generated;
using TodoList.Application.Common.Errors;
using TodoList.Application.TodoItems.Queries.GetTodoItems;

namespace TodoList.Api.Controllers
{
    [ApiController]
    public class TodoItemsController(IErrorHelper errorHelper, IMapper mapper, ISender sender, ILogger<TodoItemsController> logger) : TodoItemsControllerBase
    {
        private readonly IErrorHelper _errorHelper = errorHelper ?? throw new ArgumentNullException(nameof(errorHelper));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        private readonly ILogger<TodoItemsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public override async Task<ActionResult<ICollection<TodoItem>>> GetTodoItems(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _sender
                .Send(new GetTodoItemsQuery(), cancellationToken);

            var todoItems = _mapper
                .Map<IEnumerable<TodoItem>>(result.Value!.TodoItems);
                
            return Ok(todoItems);                
        }

        public override async Task<ActionResult<TodoItem>> GetTodoItem(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _sender
                .Send(new GetTodoItemQuery(id), cancellationToken);

            if (result is { IsError: true })
            {
                switch (result.Error)
                {
                    case NotFoundError notFoundError:
                        return _errorHelper.NotFoundErrorResult(notFoundError);
                    case ValidationError validationError:
                        return _errorHelper.ValidationErrorResult(validationError);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var todoItem = _mapper
                .Map<TodoItem>(result.Value!.TodoItem);

            return Ok(todoItem);
        }

        public override async Task<IActionResult> PutTodoItem(Guid id, TodoItem body, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (id != body.Id)
            {
                return _errorHelper.IdMismatchValidationError();
            }

            var result = await _sender
                .Send(new UpdateTodoItemCommand(body.Id, body.Description, body.IsCompleted), cancellationToken);

            if (result is { IsError: true })
            {
                switch (result.Error)
                {
                    case NotFoundError notFoundError:
                        return _errorHelper.NotFoundErrorResult(notFoundError);
                    case ValidationError validationError:
                        return _errorHelper.ValidationErrorResult(validationError);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return NoContent();          
        }

        public override async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem body, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _sender
                .Send(new CreateTodoItemCommand(body.Id, body.Description, body.IsCompleted), cancellationToken);

            if (result is { IsError: true })
            {
                switch (result.Error)
                {
                    case DuplicateError duplicateError:
                        return _errorHelper.DuplicateErrorResult(duplicateError);
                    case ValidationError validationError:
                        return  _errorHelper.ValidationErrorResult(validationError);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var createdTodoItem = _mapper
                .Map<TodoItem>(result.Value!.TodoItem);
       
            return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
        }
    }
}
