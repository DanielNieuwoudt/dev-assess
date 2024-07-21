using Microsoft.EntityFrameworkCore;
using TodoList.Domain.TodoItems;

namespace TodoList.Infrastructure.Persistence
{
    public class TodoListDbContext(DbContextOptions<TodoListDbContext> options) : DbContext(options)
    {
        public DbSet<TodoItem> TodoItems { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
        }
    }
}
