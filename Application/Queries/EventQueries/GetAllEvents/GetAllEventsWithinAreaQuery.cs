using Domain.Models;
using MediatR;

namespace Application.Queries.EventQueries.GetAllEvents
{
    public sealed record GetAllEventsWithinAreaQuery(int AllowedDistance, string UserId) : IRequest<OperationResult<List<Event>>>;
}
