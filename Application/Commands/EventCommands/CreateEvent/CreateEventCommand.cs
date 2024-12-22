using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.CreateEvent
{
    public class CreateEventCommand : IRequest<OperationResult<Event>>
    {
        public CreateEventCommand(CreateEventDto eventDto, string userId)
        {
            EventDto = eventDto;
            UserId = userId;
        }
        public CreateEventDto EventDto { get; set; }
        public string UserId { get; set; }
    }
}
