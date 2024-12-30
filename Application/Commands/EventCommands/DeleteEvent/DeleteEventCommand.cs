using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<OperationResult<Guid>>
    {
        public Guid EventId { get; set; }
        public DeleteEventCommand(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
