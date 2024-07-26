using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.TodoItems.Errors;

namespace TodoList.Application.TodoItems.Queries.GetTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record GetTodoItemQuery(Guid Id) : IRequest<TodoItemResult<ApplicationError, GetTodoItemResponse>>;
}