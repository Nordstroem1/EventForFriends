using Application.Dtos;
using Application.Dtos.Comment;
using Application.Dtos.Event;
using Application.Dtos.User;
using AutoMapper;
using Domain.Models;

namespace Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Event, CreateEventDto>().ReverseMap();
            CreateMap<CreateUserDto, User>();
        }
    }
}
