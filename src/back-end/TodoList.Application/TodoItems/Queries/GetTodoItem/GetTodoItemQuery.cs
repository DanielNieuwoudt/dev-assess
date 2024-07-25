using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common;

namespace TodoList.Application.TodoItems.Queries.GetTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record GetTodoItemQuery(Guid Id) : IRequest<Result<ApplicationError, GetTodoItemResponse>>;
}