using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.TodoItems.Errors;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record GetTodoItemsQuery : IRequest<TodoItemResult<ApplicationError, GetTodoItemsResponse>>;