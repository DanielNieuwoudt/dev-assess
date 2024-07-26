using TodoList.Domain.TodoItems;

namespace TodoList.Application.Contracts
{
    public interface ITodoItemsWriteRepository
    {
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);

        Task UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);
    }
}
