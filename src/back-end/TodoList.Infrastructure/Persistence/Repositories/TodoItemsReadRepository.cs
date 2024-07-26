using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Infrastructure.Persistence.Repositories
{
    public sealed class TodoItemsReadRepository(TodoListDbContext dbContext, ILogger<TodoItemsReadRepository> logger)
        : ITodoItemsReadRepository
    {
        private readonly TodoListDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly ILogger<TodoItemsReadRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<bool> FindByIdAsync(TodoItemId todoItemId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding todo item with Id: {Id}", todoItemId.Value);

            return await _dbContext.TodoItems.AnyAsync(ti => ti.Id == todoItemId, cancellationToken);
        }

        public async Task<bool> FindByDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding todo item with Description: {Description}", description);

            return await _dbContext.TodoItems.AnyAsync(ti => ti.Description == description && ti.IsCompleted == false, cancellationToken);
        }

        public async Task<TodoItem?> GetTodoItemAsync(TodoItemId todoItemId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving todo item with Id: {Id}", todoItemId.Value);

            return await _dbContext
                .TodoItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ti => ti.Id == todoItemId, cancellationToken);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving todo items.");

            return await _dbContext
                .TodoItems
                .AsNoTracking()
                .Where(ti => ti.IsCompleted == false)
                .ToListAsync(cancellationToken);
        }
    }
}
