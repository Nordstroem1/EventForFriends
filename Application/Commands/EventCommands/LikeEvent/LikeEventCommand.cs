using Application.Dtos.Event;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.LikeEvent
{
    public class LikeEventCommand : IRequest<OperationResult<int>>
    {
        public LikeEventDto LikeEventDto { get; set; }
        public LikeEventCommand(LikeEventDto likeEventDto)
        {
            LikeEventDto = likeEventDto;
        }
    }
}