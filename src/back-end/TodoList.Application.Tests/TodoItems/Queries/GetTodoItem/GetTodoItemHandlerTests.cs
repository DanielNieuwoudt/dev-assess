using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Queries.GetTodoItem;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Queries.GetTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class GetTodoItemHandlerTests
    {
        private readonly Mock<ITodoItemsReadRepository> _readRepositoryMock = new ();
        private readonly NullLogger<GetTodoItemHandler> _nullLogger = new ();

        [Fact]
        public void Given_NullRepository_When_GetTodoItemHandlerInitialised_Then_ThrowArgumentNullException()
        {
            var act = () => new GetTodoItemHandler(null!, _nullLogger);

            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenNullLogger_When_GetTodoItemHandlerInitialised_Then_ThrowArgumentNullException()
        {
            var act = () => new GetTodoItemHandler(_readRepositoryMock.Object, null!);

            act
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_GetTodoItemQuery_When_RepositoryTodoItemFound_Then_ReturnsGetTodoItemResponse()
        {
            var query = new GetTodoItemQuery(Guid.NewGuid());
            var todoItem = new TodoItem(new TodoItemId(Guid.NewGuid()), "", false, DateTimeOffset.Now,
                DateTimeOffset.Now);

            _readRepositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItem);
            
            var handler = new GetTodoItemHandler(_readRepositoryMock.Object, _nullLogger);

            var result = await handler
                .Handle(query, CancellationToken.None);

            result.Value
                .Should()
                .NotBeNull();

            result.Value
                .Should()
                .BeOfType<GetTodoItemResponse>();

            result.Value!.TodoItem
                .Should()
                .Be(todoItem);

            _readRepositoryMock.Verify(x => x.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_GetTodoItemQuery_When_RepositoryTodoItemNotFound_Then_ReturnsNotFoundError()
        {
            var id = Guid.NewGuid();
            var query = new GetTodoItemQuery(id);
            var expectedError = new NotFoundError("Id", id.ToString());

            _readRepositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoItem)null!);
            
            var handler = new GetTodoItemHandler(_readRepositoryMock.Object, _nullLogger);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Error
                .Should()
                .NotBeNull();
            
            result.Error
                .Should()
                .BeOfType<NotFoundError>();

            result.Error
                .Should()
                .BeEquivalentTo(expectedError);

            _readRepositoryMock.Verify(x => x.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
