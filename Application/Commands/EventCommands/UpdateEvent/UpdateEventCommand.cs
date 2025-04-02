
using Application.Dtos.Event;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.UpdateEvent
{
    public class UpdateEventCommand : IRequest<OperationResult<UpdateEventDto>>
    {
        public string UserId { get; set; }
        public string EventId { get; set; }
        public UpdateEventDto UpdateEventDto { get; set; }
        public UpdateEventCommand(string userId ,string eventID, UpdateEventDto updateEventDto)
        {
            UserId = userId;
            EventId = eventID;
            UpdateEventDto = updateEventDto;
        }
    }
}
