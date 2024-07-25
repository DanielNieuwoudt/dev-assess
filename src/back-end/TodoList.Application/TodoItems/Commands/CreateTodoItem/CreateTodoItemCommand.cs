using System.Diagnostics.CodeAnalysis;
using MediatR;
using TodoList.Application.Common.Errors;
using TodoList.Application.Common;

namespace TodoList.Application.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Record")]
    public sealed record CreateTodoItemCommand(Guid Id, string Description, bool isCompleted)
        : IRequest<Result<ApplicationError, CreateTodoItemResponse>>;

}
