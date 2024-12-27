using AutoMapper.Configuration;
using MyToDo.api.Context;
using MyToDo.Shared.Dtos;

namespace MyToDo.api.Extension
{
    public class AutoMapperProFile:MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
