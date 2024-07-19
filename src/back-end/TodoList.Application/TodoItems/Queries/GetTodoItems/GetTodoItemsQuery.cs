using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace TodoList.Application.TodoItems.Queries.GetTodoItems;

[ExcludeFromCodeCoverage(Justification = "Record")]
public sealed record GetTodoItemsQuery : IRequest<GetTodoItemsResult>;