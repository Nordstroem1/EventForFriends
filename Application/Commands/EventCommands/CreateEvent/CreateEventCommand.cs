using Application.Dtos.Event;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.CreateEvent
{
    public class CreateEventCommand : IRequest<OperationResult<Event>>
    {
        public CreateEventDto EventDto { get; set; }
        public string UserId { get; set; }
        public CreateEventCommand(CreateEventDto eventDto, string userId)
        {
            EventDto = eventDto;
            UserId = userId;
        }
    }
}
