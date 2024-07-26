using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Contracts
{
    public interface ITodoItemsReadRepository
    {
        Task<bool> FindByIdAsync(TodoItemId todoItemId, CancellationToken cancellationToken);

        Task<bool> FindByDescriptionAsync(string description, CancellationToken cancellationToken);

        Task<TodoItem?> GetTodoItemAsync(TodoItemId todoItemId, CancellationToken cancellationToken);

        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(CancellationToken cancellationToken);
    }
}
