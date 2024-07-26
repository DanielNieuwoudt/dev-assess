using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.TodoItems.Errors;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record CreateTodoItemCommand(Guid Id, string Description, bool isCompleted)
        : IRequest<TodoItemResult<ApplicationError, CreateTodoItemResponse>>;

}
