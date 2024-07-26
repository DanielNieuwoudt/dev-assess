using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.Common.Errors;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Commands.UpdateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class UpdateTodoItemHandlerTests
    {
        private readonly Mock<ITodoItemsReadRepository> _readRepositoryMock = new ();
        private readonly Mock<ITodoItemsWriteRepository> _writeRepositoryMock = new ();
        private readonly NullLogger<UpdateTodoItemHandler> _nullLogger = new ();

        [Fact]
        public void Given_NullReadRepository_When_UpdateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new UpdateTodoItemHandler(null!, _writeRepositoryMock.Object, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullWriteRepository_When_UpdateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new UpdateTodoItemHandler(_readRepositoryMock.Object, null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_UpdateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new UpdateTodoItemHandler(_readRepositoryMock.Object, _writeRepositoryMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_PutTodoItem_When_TodoItemNotFound_Then_ReturnsNotFoundError()
        {
            var handler = new UpdateTodoItemHandler(_readRepositoryMock.Object, _writeRepositoryMock.Object, _nullLogger);
            var request = new UpdateTodoItemCommand(Guid.NewGuid(), "Test", false);
            var expectedError = new NotFoundError(new Dictionary<string, string[]>());

            _readRepositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoItem)null!);

            var result = await handler.Handle(request, CancellationToken.None);
            
            result.Error
                .Should()
                .NotBeNull();

            result.Error
                .Should()
                .BeEquivalentTo(expectedError, options =>
                {
                    return options.Excluding(e => e.errors);
                });

            _writeRepositoryMock.Verify(r => r.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_PutTodoItem_When_TodoItemFound_Then_UpdatesTodoItem()
        {
            var id = Guid.NewGuid();
            var handler = new UpdateTodoItemHandler(_readRepositoryMock.Object, _writeRepositoryMock.Object, _nullLogger);
            var request = new UpdateTodoItemCommand(id, "Test", false);

            _readRepositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TodoItem(new TodoItemId(id), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now));

            await handler.Handle(request, CancellationToken.None);
            
            _writeRepositoryMock.Verify(r => r.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
