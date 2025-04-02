using Domain.Models;
using MediatR;

namespace Application.Queries.EventQueries.GetEventById
{
    public class GetEventbyIdQuery : IRequest<OperationResult<Event>>
    {
        public string EventId { get; set; }
        public GetEventbyIdQuery(string eventId)
        {
            EventId = eventId;
        }
    }
}
