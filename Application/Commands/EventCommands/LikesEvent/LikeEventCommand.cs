using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.LikesEvent
{
    public class LikeEventCommand : IRequest<OperationResult<int>>
    {
        public EventLikes _eventLikes { get; set; }
        public LikeEventCommand(EventLikes eventLikes)
        {
            _eventLikes = eventLikes;
        }
    }
}