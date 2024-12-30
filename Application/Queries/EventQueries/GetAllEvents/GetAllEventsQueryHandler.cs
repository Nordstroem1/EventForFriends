using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.EventQueries.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, OperationResult<List<Event>>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<GetAllEventsQueryHandler> _logger;
        public GetAllEventsQueryHandler(IGenericRepository<Event> eventRepository, ILogger<GetAllEventsQueryHandler> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }
        public async Task<OperationResult<List<Event>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepository.GetAllAsync();

            if(result == null || result.Count() <= 0)
            {
                _logger.LogError("No events in List");
                return OperationResult<List<Event>>.Fail("No events in list", "Application");
            }

            return OperationResult<List<Event>>.Success(result.ToList());
        }
    }
}
