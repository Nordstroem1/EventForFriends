using Domain.Models;
using MediatR;

namespace Application.Queries.EventQueries.GetEventById
{
    public class GetEventbyIdQuery : IRequest<OperationResult<Event>>
    {
        public Guid EventId { get; set; }
        public GetEventbyIdQuery(Guid eventId)
        {
            EventId = eventId;
        }
    }
}
