using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.EventQueries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventbyIdQuery, OperationResult<Event>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<GetEventByIdQueryHandler> _logger; 
        public GetEventByIdQueryHandler(IGenericRepository<Event> eventRepository, ILogger<GetEventByIdQueryHandler> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }
        public async Task<OperationResult<Event>> Handle(GetEventbyIdQuery request, CancellationToken cancellationToken)
        {
            if (request.EventId == Guid.Empty || request == null)
            {
                _logger.LogError("No id was given");
                return OperationResult<Event>.Fail("No id was given", "Application");
            }

            var result = await _eventRepository.GetByIdAsync(request.EventId);

            if (result == null)
            {
                _logger.LogError($"No user with id {request.EventId}");
                return OperationResult<Event>.Fail("No user with that id", "Application");
            }

            return OperationResult<Event>.Success(result);
        }
    }
}
