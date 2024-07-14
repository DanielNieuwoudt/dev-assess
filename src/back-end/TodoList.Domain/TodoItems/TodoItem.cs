using TodoList.Domain.Common.Models;
using TodoList.Domain.TodoItems.ValueObjects;

namespace TodoList.Domain.TodoItems
{
    public sealed class TodoItem :  AggregateRoot<TodoItemId>
    {
        public TodoItem(TodoItemId id, string description, bool isCompleted, DateTimeOffset createdAt, DateTimeOffset modifiedAt) : base(id)
        {
            Description = description;
            IsCompleted = isCompleted;
            CreatedAt = createdAt;
            ModifiedAt = modifiedAt;
        }

        public string Description { get; private set; }

        public bool IsCompleted { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset ModifiedAt { get; private set; }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }

        public void MarkAsInCompleted()
        {
            IsCompleted = true;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetModified()
        {
            ModifiedAt = DateTimeOffset.Now;
        }

        #pragma warning disable CS8618
        private TodoItem()
        {
        }
        #pragma warning restore CS8618

    }
}
