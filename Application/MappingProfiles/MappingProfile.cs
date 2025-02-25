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
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, CreateUserDto>()
                           .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "User"))
                           .ReverseMap(); 
        }
    }
}
