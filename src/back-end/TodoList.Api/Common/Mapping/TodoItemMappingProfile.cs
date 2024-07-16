using AutoMapper;

namespace TodoList.Api.Common.Mapping
{
    public class TodoItemMappingProfile : Profile
    {
        public TodoItemMappingProfile()
        {
            CreateMap<Domain.TodoItems.TodoItem, Generated.TodoItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted));
        }
    }
}
