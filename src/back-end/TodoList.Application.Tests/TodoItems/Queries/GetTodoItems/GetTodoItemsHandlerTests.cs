using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Queries.GetTodoItems;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Queries.GetTodoItems
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class GetTodoItemsHandlerTests
    {
        
        private readonly Mock<ITodoItemsReadRepository> _readRepositoryMock = new();
        private readonly NullLogger<GetTodoItemsHandler> _nullLogger = new();

        [Fact]
        public void Given_Repository_When_HandlerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new GetTodoItemsHandler(null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_HandlerInitialised_Then_ThrowsArgumentNullException()
        {
            var action = () => new GetTodoItemsHandler(_readRepositoryMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_Request_When_Handle_Then_ReturnsTodoItems()
        {
            var todoItems = new List<TodoItem>
            {
                new (new TodoItemId(Guid.NewGuid()), "Test 1", false, DateTimeOffset.Now, DateTimeOffset.Now),
                new (new TodoItemId(Guid.NewGuid()), "Test 1", false, DateTimeOffset.Now, DateTimeOffset.Now)
            };

            _readRepositoryMock
                .Setup(x => x.GetTodoItemsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItems);

            var handler = new GetTodoItemsHandler(_readRepositoryMock.Object, new NullLogger<GetTodoItemsHandler>());

            var result = await handler.Handle(new GetTodoItemsQuery(), CancellationToken.None);

            result.Value
                .Should()
                .NotBeNull();

            result.Value
                .Should()
                .BeOfType<GetTodoItemsResponse>();

            result.Value!.TodoItems
                .Should()
                .BeEquivalentTo(todoItems);

            _readRepositoryMock.Verify(x => x.GetTodoItemsAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
