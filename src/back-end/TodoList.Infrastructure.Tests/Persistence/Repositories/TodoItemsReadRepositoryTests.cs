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
    public class TodoItemsReadRepositoryTests : IDisposable
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;

        public TodoItemsReadRepositoryTests()
        {
            _serviceCollection
                .AddDbContext<TodoListDbContext>(opt => opt.UseInMemoryDatabase("TodoReadInMemoryDb"));

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public void Given_NullDbContext_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsReadRepository(null!, new NullLogger<TodoItemsReadRepository>());

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_TodoItemsRepositoryInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new TodoItemsReadRepository(_serviceProvider.GetRequiredService<TodoListDbContext>(), null!);

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

            var readRepository = new TodoItemsReadRepository(dbContext, new NullLogger<TodoItemsReadRepository>());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now);

            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();

            var result = await readRepository
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

            var readRepository = new TodoItemsReadRepository(dbContext, new NullLogger<TodoItemsReadRepository>());

            var result = await readRepository
                .GetTodoItemAsync(new TodoItemId(Guid.NewGuid()), CancellationToken.None);

            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task Given_TodoItems_When_GetTodoItemsAsync_Then_ReturnsNotCompletedTodoItems()
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var readRepository = new TodoItemsReadRepository(dbContext, new NullLogger<TodoItemsReadRepository>());

            var itemOne = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test 1", false, DateTimeOffset.Now,
                DateTimeOffset.Now);
            var itemTwo = new TodoItem(new TodoItemId(Guid.NewGuid()), "Test 2", true, DateTimeOffset.Now,
                DateTimeOffset.Now);
            
            dbContext.TodoItems.Add(itemOne);
            dbContext.TodoItems.Add(itemTwo);
            
            await dbContext.SaveChangesAsync();

            var result = (await readRepository.GetTodoItemsAsync(CancellationToken.None))
                .ToList();

            result
                .Should()
                .NotBeNull();

            result
                .Count()
                .Should()
                .Be(1);

            result
                .Should()
                .BeEquivalentTo(new List<TodoItem>() { itemOne });
        }

        [Theory]
        [MemberData(nameof(FindByIdTodoItemData))]
        public async Task Given_ExistingTodoItem_When_FindById_Then_ReturnsExpectedResult(Guid id, TodoItem todoItem, bool expectedResult)
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var readRepository = new TodoItemsReadRepository(dbContext, new NullLogger<TodoItemsReadRepository>());
            
            dbContext.TodoItems.Add(todoItem);
            await dbContext.SaveChangesAsync();

            var result = await readRepository
                .FindByIdAsync(new TodoItemId(id), CancellationToken.None);

            result
                .Should()
                .Be(expectedResult);
        }

        public static IEnumerable<object[]> FindByIdTodoItemData =>
            [
                [
                    Guid.Parse("9998532a-8761-4e1d-83eb-ba55e478e640") ,
                    new TodoItem( new TodoItemId(Guid.Parse("9998532a-8761-4e1d-83eb-ba55e478e640")), "Found - Incomplete", false, DateTimeOffset.Now, DateTimeOffset.Now), 
                    true
                ],
                [
                    Guid.Parse("08f92c29-3493-493e-aead-92c5e1fbae8a") ,
                    new TodoItem( new TodoItemId(Guid.Parse("08f92c29-3493-493e-aead-92c5e1fbae8a")), "Found - Complete", true, DateTimeOffset.Now, DateTimeOffset.Now), 
                    true
                ],
                [
                    Guid.NewGuid(),
                    new TodoItem(new TodoItemId(Guid.NewGuid()), "Not Found", false, DateTimeOffset.Now, DateTimeOffset.Now), 
                    false
                ]
            ];

        [Theory]
        [InlineData("Found", false, true)]
        [InlineData("NotFound", true, false)]
        public async Task Given_ExistingTodoItem_When_FindByDescription_Then_ReturnsExpectedResult(string description, bool isComplete, bool expectedResult)
        {
            using var scope = _serviceProvider
                .CreateScope();
            
            var dbContext = scope
                .ServiceProvider
                .GetRequiredService<TodoListDbContext>();

            var readRepository = new TodoItemsReadRepository(dbContext, new NullLogger<TodoItemsReadRepository>());

            dbContext.TodoItems.Add(new TodoItem(new TodoItemId(Guid.Parse("9998532a-8761-4e1d-83eb-ba55e478e640")),
                description, isComplete, DateTimeOffset.Now, DateTimeOffset.Now));
            await dbContext.SaveChangesAsync();

            var result = await readRepository
                .FindByDescriptionAsync(description, CancellationToken.None);

            result
                .Should()
                .Be(expectedResult);
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
