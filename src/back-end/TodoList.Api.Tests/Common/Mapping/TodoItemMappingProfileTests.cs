using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using FluentAssertions;
using TodoList.Api.Common.Mapping;
using TodoList.Domain.TodoItems.ValueObjects;
using Xunit;

namespace TodoList.Api.Tests.Common.Mapping
{
    [ExcludeFromCodeCoverage(Justification = "Tests")]
    public class TodoItemMappingProfileTests
    {
        private readonly IMapper _mapper;

        public TodoItemMappingProfileTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoItemMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Fact]
        public void Given_MappingProfile_When_Configured_Then_ShouldHaveValidConfiguration()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Given_TodoItem_When_MappedToTodoItemDto_Then_ShouldHaveValidMapping()
        {
            var id = Guid.NewGuid();
            var description = "Description";
            var isCompleted = false;

            var domainItem = new Domain.TodoItems.TodoItem(new TodoItemId(id), description, isCompleted, DateTimeOffset.Now, DateTimeOffset.Now);
            var contractItem = new Generated.TodoItem
            {
                Id = id,
                Description = description,
                IsCompleted = isCompleted
            };

            var resultingItem = _mapper
                .Map<Generated.TodoItem>(domainItem);

            resultingItem
                .Should()
                .BeEquivalentTo(contractItem);
        }
    }
}
