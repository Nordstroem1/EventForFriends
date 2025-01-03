using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.CreateEvent
{
    public class CreateCommandHandler : IRequestHandler<CreateEventCommand, OperationResult<Event>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreateCommandHandler> _logger;
        private readonly IGenericRepository<Event> _eventRepository;
        public CreateCommandHandler(UserManager<IdentityUser> userManager, ILogger<CreateCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<OperationResult<Event>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.EventDto == null)
                {
                    _logger.LogError("Event data is required");
                    return OperationResult<Event>.Fail("Event data is required", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserId);

                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Event>.Fail("User not found", "Applicaton");
                }

                var newEvent = new Event
                {
                    EventId = Guid.NewGuid(),
                    EventName = request.EventDto.EventName,
                    Description = request.EventDto.Description,
                    Location = request.EventDto.Location,
                    StartDate = request.EventDto.StartDate,
                    EndDate = request.EventDto.EndDate,
                    CreatedBy = request.UserId
                };

                await _eventRepository.AddAsync(newEvent);
                _logger.LogInformation("Event created successfully");

                return OperationResult<Event>.Success(newEvent);
            }
            catch
            {
                return OperationResult<Event>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
