using MediatR;

namespace TodoList.Application.TodoItems.Commands.UpdateTodoItem
{
    public sealed record UpdateTodoItemCommand(Guid RouteId, Guid Id, string Description, bool IsCompleted ) : IRequest;
}
