using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Queries.EventQueries.GetAllEvents
{
    public class GetAllEventsQueryHandler(IGenericRepository<Event> eventRepository, ILogger<GetAllEventsQueryHandler> logger, UserManager<User> usermanager) : IRequestHandler<GetAllEventsQuery, OperationResult<List<Event>>>
    {
        private readonly IGenericRepository<Event> _eventRepository = eventRepository;
        private readonly ILogger<GetAllEventsQueryHandler> _logger = logger;
        private readonly UserManager<User> _userManager = usermanager;

        public async Task<OperationResult<List<Event>>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var result = await _eventRepository.GetAllAsync();
            
            foreach (var eventEntity in result)
            {
                var user = await _userManager.FindByIdAsync(eventEntity.CreatedBy);
                if (user != null)
                {
                    eventEntity.CreatedBy = user.UserName!;
                }
            }

            if (result == null || result.Count() <= 0)
            {
                _logger.LogError("No events in List");
                return OperationResult<List<Event>>.Fail("No events in list", "Application");
            }

            return OperationResult<List<Event>>.Success(result.ToList());
        }
    }
}
