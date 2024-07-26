using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TodoList.Application.TodoItems.Errors;
using TodoList.Application.Contracts;
using TodoList.Application.TodoItems.Commands.CreateTodoItem;
using TodoList.Domain.TodoItems;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Application.Tests.TodoItems.Commands.CreateTodoItem
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class CreateTodoItemHandlerTests
    {
        private readonly Mock<ITodoItemsRepository> _repositoryMock = new ();
        private readonly NullLogger<CreateTodoItemHandler> _nullLogger = new ();

        [Fact]
        public void Given_NullRepository_When_CreateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new CreateTodoItemHandler(null!, _nullLogger);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public void Given_NullLogger_When_CreateTodoItemHandlerInitialised_ThenThrowsArgumentNullException()
        {
            var action = () => new CreateTodoItemHandler(_repositoryMock.Object, null!);

            action
                .Should()
                .Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_NoDuplicateTodoItemFound_Then_ReturnsCreateTodoItemResponse()
        {
            var id = Guid.NewGuid();

            var command = new CreateTodoItemCommand(id, "Description", false);
            var todoItem = new TodoItem(new TodoItemId(id), "Description", false, DateTimeOffset.Now, DateTimeOffset.Now);

            _repositoryMock
                .Setup(r => r.FindByIdAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.FindByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            
            _repositoryMock
                .Setup(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(todoItem);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var result = await handler
                .Handle(command, CancellationToken.None);

            result.Value
                .Should()
                .NotBeNull();

            result.Value!.TodoItem
                .Should()
                .Be(todoItem);

            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_DuplicateTodoItemFoundByDescription_Then_ReturnsDuplicateError()
        {
            var id = Guid.NewGuid();
            var command = new CreateTodoItemCommand(id, "Description", false);
            var expectedError = new DuplicateError(new Dictionary<string, string[]>());

            _repositoryMock
                .Setup(r => r.FindByIdAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _repositoryMock
                .Setup(r => r.FindByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Error
                .Should()
                .NotBeNull();

            result.Error
                .Should()
                .BeEquivalentTo(expectedError, options =>
                {
                    return options.Excluding(e => e.errors);
                });
     
            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Given_CreateTodoItemCommand_When_DuplicateTodoItemFoundById_Then_ReturnsDuplicateError()
        {
            var id = Guid.NewGuid();
            var command = new CreateTodoItemCommand(id, "Description", false);
            var expectedError = new DuplicateError(new Dictionary<string, string[]>());

            _repositoryMock
                .Setup(r => r.FindByIdAsync(It.IsAny<TodoItemId>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _repositoryMock
                .Setup(r => r.FindByDescriptionAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = new CreateTodoItemHandler(_repositoryMock.Object, _nullLogger);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Error
                .Should()
                .NotBeNull();

            result.Error
                .Should()
                .BeEquivalentTo(expectedError, options =>
                {
                    return options.Excluding(e => e.errors);
                });

            _repositoryMock.Verify(r => r.CreateTodoItemAsync(It.IsAny<TodoItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
