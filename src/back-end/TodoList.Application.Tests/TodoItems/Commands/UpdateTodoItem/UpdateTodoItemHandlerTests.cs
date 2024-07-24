using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.Common.Exceptions;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Commands.UpdateTodoItem;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Commands.UpdateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class UpdateTodoItemHandlerTests
    {
        private readonly Mock<ITodoItemsRepository> _repositoryMock = new ();
        private readonly NullLogger<UpdateTodoItemHandler> _nullLogger = new ();

        [Fact]
        public void Given_NullRepository_When_UpdateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new UpdateTodoItemHandler(null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_UpdateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new UpdateTodoItemHandler(_repositoryMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_PutTodoItem_When_TodoItemNotFound_Then_ThrowsTodoItemNotFoundException()
        {
            var handler = new UpdateTodoItemHandler(_repositoryMock.Object, _nullLogger);
            var request = new UpdateTodoItemCommand(Guid.NewGuid(), "Test", false);

            _repositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TodoItem)null!);

            var action = async () => await handler.Handle(request, CancellationToken.None);
            
            await action
                .Should()
                .ThrowAsync<TodoItemNotFoundException>();

            _repositoryMock.Verify(r => r.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_PutTodoItem_When_TodoItemFound_Then_UpdatesTodoItem()
        {
            var id = Guid.NewGuid();
            var handler = new UpdateTodoItemHandler(_repositoryMock.Object, _nullLogger);
            var request = new UpdateTodoItemCommand(id, "Test", false);

            _repositoryMock
                .Setup(r => r.GetTodoItemAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new TodoItem(new TodoItemId(id), "Test", false, DateTimeOffset.Now, DateTimeOffset.Now));

            await handler.Handle(request, CancellationToken.None);
            
            _repositoryMock.Verify(r => r.UpdateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
