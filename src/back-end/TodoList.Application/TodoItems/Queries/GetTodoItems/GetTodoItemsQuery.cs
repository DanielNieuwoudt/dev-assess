using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record GetTodoItemsQuery : IRequest<Result<ApplicationError, GetTodoItemsResponse>>;