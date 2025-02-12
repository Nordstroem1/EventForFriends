using Application.Dtos.Event;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, OperationResult<UpdateEventDto>>
    {
        private readonly IGenericRepository<Event> _eventRepository;   
        private readonly ILogger<UpdateEventCommandHandler> _logger;
        private readonly UserManager<User> _userManager;
        public UpdateEventCommandHandler(IGenericRepository<Event> eventRepository, ILogger<UpdateEventCommandHandler> logger, UserManager<User> userManager)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<OperationResult<UpdateEventDto>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.UpdateEventDto == null)
                {
                    _logger.LogError("Event data is required");
                    return OperationResult<UpdateEventDto>.Fail("Event data is required", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UpdateEventDto.CreatedBy);
                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<UpdateEventDto>.Fail("User not found", "Application");
                }

                var foundEvent = await _eventRepository.GetByIdAsync(request.EventId);
                if (foundEvent == null)
                {
                    _logger.LogError("Event not found");
                    return OperationResult<UpdateEventDto>.Fail("Event not found", "Applicaton");
                }

                foundEvent.EventName = request.UpdateEventDto.EventName;
                foundEvent.Description = request.UpdateEventDto.Description;
                foundEvent.Location = request.UpdateEventDto.Location;
                foundEvent.StartDate = request.UpdateEventDto.StartDate;
                foundEvent.EndDate = request.UpdateEventDto.EndDate;
                foundEvent.IsclosedEvent = request.UpdateEventDto.IsclosedEvent;
                await _eventRepository.UpdateAsync(foundEvent);
                _logger.LogInformation("Event updated successfully");

                return OperationResult<UpdateEventDto>.Success(request.UpdateEventDto);
            }
            catch
            {
                return OperationResult<UpdateEventDto>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
