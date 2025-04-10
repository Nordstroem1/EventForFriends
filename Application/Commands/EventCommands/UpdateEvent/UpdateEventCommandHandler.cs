using Application.Dtos.Event;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, OperationResult<Event>>
    {
        private readonly IGenericRepository<Event> _eventRepository;   
        private readonly ILogger<UpdateEventCommandHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public UpdateEventCommandHandler(IGenericRepository<Event> eventRepository, ILogger<UpdateEventCommandHandler> logger, UserManager<User> userManager, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<OperationResult<Event>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UpdateEventDto == null)
                {
                    _logger.LogError("Event data is required");
                    return OperationResult<Event>.Fail("Event data is required", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserId);
                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Event>.Fail("User not found", "Application");
                }

                var foundEvent = await _eventRepository.GetByIdAsync(request.EventId);
                if (foundEvent == null)
                {
                    _logger.LogError("Event not found");
                    return OperationResult<Event>.Fail("Event not found", "Applicaton");
                }

                if(foundUser.Id != foundEvent.CreatedBy &&
                    foundUser.Role.ToLower() == "superadmin" &&
                    foundUser.Role.ToLower() == "admin")
                {
                    _logger.LogError("User does not have permission to update this event");
                    return OperationResult<Event>.Fail("User does not have permission to update this event", "Applicaton");
                }

                var updatedEvent = _mapper.Map(request.UpdateEventDto, foundEvent);

                await _eventRepository.UpdateAsync(updatedEvent);
                _logger.LogInformation("Event updated successfully");

                return OperationResult<Event>.Success(updatedEvent);
            }
            catch
            {
                return OperationResult<Event>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
