using Domain.Models;
using MediatR;

namespace Application.Commands.EventCommands.LikesEvent
{
    public sealed record LikeEventCommand(string UserId,string EventId) : IRequest<OperationResult<int>>;
}