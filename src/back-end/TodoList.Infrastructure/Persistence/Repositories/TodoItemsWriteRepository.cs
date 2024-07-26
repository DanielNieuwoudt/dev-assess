using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;

namespace TodoList.Infrastructure.Persistence.Repositories
{
    public class TodoItemsWriteRepository(TodoListDbContext dbContext, ILogger<TodoItemsReadRepository> logger) : ITodoItemsWriteRepository
    {
        private readonly TodoListDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly ILogger<TodoItemsReadRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating todo item with Id: {Id}", todoItem.Id.Value);

            await _dbContext.TodoItems.AddAsync(todoItem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created todo item with Id: {Id}", todoItem.Id.Value);

            return todoItem;
        }

        public async Task UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating todo item with Id: {Id}", todoItem.Id.Value);

            _dbContext.Entry(todoItem).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated todo item with Id: {Id}", todoItem.Id.Value);
        }
    }
}
