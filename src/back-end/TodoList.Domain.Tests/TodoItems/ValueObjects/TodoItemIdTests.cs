using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Domain.Tests.TodoItems.ValueObjects
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemIdTests
    {
        [Fact]
        public void Given_TodoItemId_When_EmptyGuid_ThenThrowArgumentException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new TodoItemId(Guid.Empty);

            action
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Given_TodoItemId_When_ValidGuid_ThenReturnsTodoItemId()
        {
            var id = Guid.NewGuid();

            var todoItemId = new TodoItemId(id);

            todoItemId.Value
                .Should()
                .Be(id);
        }
        
        [Fact]
        public void Given_TodoItemId_When_CreateWithValidGuid_ThenReturnsTodoItemId()
        {
            var id = Guid.NewGuid();

            var todoItemId = TodoItemId.Create(id);

            todoItemId.Value
                .Should()
                .Be(id);
        }

        [Fact]
        public void Given_TodoItemId_When_CreateWithEmptyGuid_ThenThrowArgumentException()
        {
            Action action = () => TodoItemId.Create(Guid.Empty);

            action
                .Should()
                .Throw<ArgumentException>();
        }

        [Fact]
        public void Given_TodoItemId_When_CompareTwoTodoItemId_ThenReturnsTrue()
        {
            var id = Guid.NewGuid();

            var todoItemId1 = new TodoItemId(id);
            var todoItemId2 = new TodoItemId(id);

            var result = todoItemId1 == todoItemId2;

            result
                .Should()
                .BeTrue();
        }
    }
}
