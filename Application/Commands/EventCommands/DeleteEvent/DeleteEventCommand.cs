using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<OperationResult<Guid>>
    {
        public string EventId { get; set; }
        public DeleteEventCommand(string eventId)
        {
            EventId = eventId;
        }
    }
}
