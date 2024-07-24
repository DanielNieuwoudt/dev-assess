using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Application.Contracts;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Infrastructure.Persistence.Repositories
{
    public sealed class TodoItemsRepository(TodoListDbContext dbContext, ILogger<TodoItemsRepository> logger)
        : ITodoItemsRepository
    {
        private readonly TodoListDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly ILogger<TodoItemsRepository> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating todo item with id {Id}.", todoItem.Id);

            await _dbContext.TodoItems.AddAsync(todoItem, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Created todo item with id {Id}.", todoItem.Id);

            return todoItem;
        }

        public async Task<bool> FindByIdAsync(TodoItemId todoItemId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding todo item with id {Id}.", todoItemId);

            return await _dbContext.TodoItems.AnyAsync(ti => ti.Id == todoItemId, cancellationToken);
        }

        public async Task<bool> FindByDescriptionAsync(string description, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding todo item with description {Description}.", description);

            return await _dbContext.TodoItems.AnyAsync(ti => ti.Description == description && ti.IsCompleted == false, cancellationToken);
        }

        public async Task<TodoItem?> GetTodoItemAsync(TodoItemId todoItemId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving todo item with id {Id}.", todoItemId);

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

        public async Task UpdateTodoItemAsync(TodoItem todoItem, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating todo item with id {Id}.", todoItem.Id);

            _dbContext.Entry(todoItem).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Updated todo item with id {Id}.", todoItem.Id);
        }
    }
}
