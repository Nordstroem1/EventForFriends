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
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<UpdateEventDto, Event>().ReverseMap();

            CreateMap<User, UserLikeDto>()
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Event, EventWithLikesDto>()
                .ForMember(dest => dest.LikeList, opt => opt.MapFrom(src => src.LikeList));

            CreateMap<Event, CreateEventDto>().ReverseMap()
            .ForMember(dest => dest.EventId, opt => opt
            .MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<CreateUserDto, User>()
                     .ForMember(dest => dest.CreatedAt, opt => opt
                     .MapFrom(src => DateTime.UtcNow));
        }
    }
}