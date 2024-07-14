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

            return todoItem;
        }

        public async Task<bool> FindDuplicateTodoItemAsync(Expression<Func<TodoItem, bool>> expression, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Finding duplicate todo items.");

            return await _dbContext.TodoItems.AnyAsync(expression, cancellationToken: cancellationToken);
        }

        public async Task<TodoItem?> GetTodoItemAsync(TodoItemId todoItemId, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving todo item with id {Id}.", todoItemId);

            return await _dbContext.TodoItems.FindAsync(todoItemId , cancellationToken);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving todo items.");

            return await _dbContext.TodoItems.ToListAsync(cancellationToken);
        }
    }
}
