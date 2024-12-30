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
            if(request.EventId == Guid.Empty)
            {
                _logger.LogError("Id is null");
                return OperationResult<Guid>.Fail("EventId can't be null","Application");
            }

            var result = await _eventRepository.GetByIdAsync(request.EventId);
            if(result == null)
            {
                _logger.LogError("Could not find event");
                return OperationResult<Guid>.Fail("Could not find event", "Application");
            }

            await _eventRepository.DeleteAsync(result);
            return OperationResult<Guid>.Success(Guid.Empty);
        }
    }
}
