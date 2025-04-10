using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public sealed record DeleteEventCommand(string UserId, string EventId) : IRequest<OperationResult<bool>>;
}
