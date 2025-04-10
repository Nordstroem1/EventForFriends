
using Application.Dtos.Event;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.UpdateEvent
{
    public sealed record UpdateEventCommand(string UserId, string EventId, UpdateEventDto UpdateEventDto) : IRequest<OperationResult<Event>>;
}
