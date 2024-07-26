using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;
using TodoList.Infrastructure.Persistence;
using TodoList.Infrastructure.Persistence.Repositories;

namespace TodoList.Infrastructure.Tests.Persistence.Repositories
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemWriteRepositoryTests : IDisposable
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;

        public TodoItemWriteRepositoryTests()
        {
            _serviceCollection
                .AddDbContext<TodoListDbContext>(opt => opt.UseInMemoryDatabase("TodoWriteInMemoryDb"));

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void Given_NullDbContext_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsWriteRepository(null!, new NullLogger<TodoItemsReadRepository>());

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsWriteRepository(_serviceProvider.GetRequiredService<TodoListDbContext>(), null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_TodoItem_When_CreateTodoItem_Then_ReturnsCreatedTodoItem()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var writeRepository = new TodoItemsWriteRepository(dbContext, new NullLogger<TodoItemsReadRepository>());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now);

            var result = await writeRepository
                .CreateTodoItemAsync(todoItem, CancellationToken.None);

            result
                .Should()
                .BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task Given_TodoItem_When_UpdateTodoItem_Then_ReturnsUpdatedTodoItem()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var writeRepository = new TodoItemsWriteRepository(dbContext, new NullLogger<TodoItemsReadRepository>());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now);

            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();

            todoItem.MarkAsCompleted();
            await writeRepository
                .UpdateTodoItemAsync(todoItem, CancellationToken.None);

        }

        public void Dispose()
        {
            _serviceProvider
                .GetRequiredService<TodoListDbContext>()
                .Database
                .EnsureDeleted();
        }
    }
}
