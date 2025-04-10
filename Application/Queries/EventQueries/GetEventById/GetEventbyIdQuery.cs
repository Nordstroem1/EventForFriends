using Domain.Models;
using MediatR;
using System.Linq.Expressions;

namespace Application.Queries.EventQueries.GetEventById
{
    public sealed record GetEventbyIdQuery(string EventId, params Expression<Func<Event, object>>[] Includes) : IRequest<OperationResult<Event>>;
}
