using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Eventing.Reader;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public class DeleteCommandHandler : IRequestHandler<DeleteEventCommand, OperationResult<Guid>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<DeleteCommandHandler> _logger;
        public DeleteCommandHandler(IGenericRepository<Event> eventRepository, ILogger<DeleteCommandHandler> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }
        public async Task<OperationResult<Guid>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.EventId == Guid.Empty)
                {
                    _logger.LogError("Id is null");
                    return OperationResult<Guid>.Fail("EventId can't be null", "Application");
                }

                var foundEvent = await _eventRepository.GetByIdAsync(request.EventId);
                if (foundEvent == null)
                {
                    _logger.LogError("Could not find foundEvent");
                    return OperationResult<Guid>.Fail("Could not find foundEvent", "Application");
                }

                var eventDeletion = await _eventRepository.DeleteAsync(foundEvent);

                if(eventDeletion != null)
                {
                    _logger.LogError("Could not delete event.");
                    return OperationResult<Guid>.Fail("Could not delete event.","Application");
                }
                _logger.LogInformation("Event deleted successfully");

                return OperationResult<Guid>.Success(Guid.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: " + ex.Message);
                return OperationResult<Guid>.Fail("Unexpected error", "Application");
            }
        }
    }
}
