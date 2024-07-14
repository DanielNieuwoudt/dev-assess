using System.Linq.Expressions;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Contracts
{
    public interface ITodoItemsRepository
    {
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken);

        Task<bool> FindDuplicateTodoItemAsync(Expression<Func<TodoItem, bool>> expression, CancellationToken cancellationToken);

        Task<TodoItem?> GetTodoItemAsync(TodoItemId todoItemId, CancellationToken cancellationToken);

        Task<IEnumerable<TodoItem>> GetTodoItemsAsync(CancellationToken cancellationToken);
    }
}
