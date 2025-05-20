using Application.Dtos.Event;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.EventCommands.CreateEvent
{
    public sealed record CreateEventCommand(string UserId, CreateEventDto EventDto, IFormFile ImageFile) : IRequest<OperationResult<Event>>;
}
