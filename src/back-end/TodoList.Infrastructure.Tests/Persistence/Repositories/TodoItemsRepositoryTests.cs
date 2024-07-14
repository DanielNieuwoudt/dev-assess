using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
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
    public class TodoItemsRepositoryTests : IDisposable
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;

        public TodoItemsRepositoryTests()
        {
            _serviceCollection
                .AddDbContext<TodoListDbContext>(opt => opt.UseInMemoryDatabase("TodoInMemoryDb"));

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public void Given_NullDbContext_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsRepository(null!, new NullLogger<TodoItemsRepository>());

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsRepository(_serviceProvider.GetRequiredService<TodoListDbContext>(), null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_ExistingTodoItemId_When_GetTodoItemAsync_Then_ReturnsTodoItem()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var repository = new TodoItemsRepository(dbContext, new NullLogger<TodoItemsRepository>());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now);

            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();

            var result = await repository
                .GetTodoItemAsync(todoItem.Id, CancellationToken.None);

            result
                .Should()
                .BeEquivalentTo(todoItem);
        }

        [Fact]
        public async Task Given_NonExistentTodoItemId_When_GetTodoItemAsync_Then_ReturnsNull()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var repository = new TodoItemsRepository(dbContext, new NullLogger<TodoItemsRepository>());

            var result = await repository
                .GetTodoItemAsync(new TodoItemId(Guid.NewGuid()), CancellationToken.None);

            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task Given_TodoItems_When_GetTodoItemsAsync_Then_ReturnsTodoItems()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var repository = new TodoItemsRepository(dbContext, new NullLogger<TodoItemsRepository>());

            var result = await repository
                .GetTodoItemsAsync(CancellationToken.None);

            result
                .Should()
                .BeEquivalentTo(new List<TodoItem>());
        }

        [Fact]
        public async Task Given_TodoItem_When_CreateTodoItem_Then_ReturnsCreatedTodoItem()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var repository = new TodoItemsRepository(dbContext, new NullLogger<TodoItemsRepository>());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now);

            var result = await repository
                .CreateTodoItemAsync(todoItem, CancellationToken.None);

            result
                .Should()
                .BeEquivalentTo(todoItem);
        }

        [Theory]
        [MemberData(nameof(FindDuplicateTodoItemData))]
        public async Task Given_ExistingTodoItem_When_FindDuplicateTodoItem_Then_ReturnsExpectedResult(TodoItem todoItem, Expression<Func<TodoItem, bool>> expression, bool expectedResult)
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var repository = new TodoItemsRepository(dbContext, new NullLogger<TodoItemsRepository>());
            
            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();

            var result = await repository
                .FindDuplicateTodoItemAsync(expression, CancellationToken.None);

            result
                .Should()
                .Be(expectedResult);
        }

        public static IEnumerable<object[]> FindDuplicateTodoItemData =>
            new List<object[]>
            {
                new object[]
                {   new TodoItem(new TodoItemId(Guid.Parse("9998532a-8761-4e1d-83eb-ba55e478e640")), "Test 1", false, DateTimeOffset.Now, DateTimeOffset.Now),
                    (Expression<Func<TodoItem, bool>>)(ti => ti.Id == new TodoItemId(Guid.Parse("9998532a-8761-4e1d-83eb-ba55e478e640"))), 
                    true
                },
                new object[]
                {   new TodoItem(new TodoItemId(Guid.NewGuid()), "Test 2", false, DateTimeOffset.Now, DateTimeOffset.Now),
                    (Expression<Func<TodoItem, bool>>)(ti => ti.Description == "Test 2"), 
                    true
                },
                new object[]
                {   new TodoItem(new TodoItemId(Guid.NewGuid()), "Test 3", true, DateTimeOffset.Now, DateTimeOffset.Now),
                    (Expression<Func<TodoItem, bool>>)(ti => ti.Description == "Test 3" && ti.IsCompleted == false), 
                    false
                }
            };

        public void Dispose()
        {
            _serviceProvider
                .GetRequiredService<TodoListDbContext>()
                .Database
                .EnsureDeleted();
        }
    }
}
