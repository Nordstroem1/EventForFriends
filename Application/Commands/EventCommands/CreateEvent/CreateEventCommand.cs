using Application.Dtos.Event;
using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.CreateEvent
{
    public sealed record CreateEventCommand(string UserId, CreateEventDto EventDto) : IRequest<OperationResult<Event>>;
}
