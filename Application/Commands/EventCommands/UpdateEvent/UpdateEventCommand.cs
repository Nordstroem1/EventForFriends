
using Application.Dtos.Event;
using Domain.Models;
using Infrastructure.Migrations;
using MediatR;

namespace Application.Commands.EventCommands.UpdateEvent
{
    public class UpdateEventCommand : IRequest<OperationResult<UpdateEventDto>>
    {
        public Guid EventId { get; set; }
        public UpdateEventDto UpdateEventDto { get; set; }
        public UpdateEventCommand(Guid eventID, UpdateEventDto updateEventDto)
        {
            EventId = eventID;
            UpdateEventDto = updateEventDto;
        }
    }
}
