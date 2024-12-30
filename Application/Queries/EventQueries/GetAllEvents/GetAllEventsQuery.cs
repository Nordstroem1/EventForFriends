using Domain.Models;
using MediatR;

namespace Application.Queries.EventQueries.GetAllEvents
{
    public class GetAllEventsQuery : IRequest<OperationResult<List<Event>>>
    {
        public List<Event> Events { get; set; }
        public GetAllEventsQuery()
        {
            Events = new List<Event>();
        }
    }
}
