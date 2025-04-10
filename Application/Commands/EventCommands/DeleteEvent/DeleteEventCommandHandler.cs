using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteEventCommandHandler> _logger;
        public DeleteEventCommandHandler(UserManager<User> userManager , IGenericRepository<Event> eventRepository, ILogger<DeleteEventCommandHandler> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<OperationResult<bool>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.EventId == string.Empty)
                {
                    _logger.LogError("Id is null");
                    return OperationResult<bool>.Fail("EventId can't be null", "Application");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserId);
                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<bool>.Fail("User not found", "Application");
                }

                var foundEvent = await _eventRepository.GetByIdAsync(request.EventId);
                if (foundEvent == null)
                {
                    _logger.LogError("Could not find foundEvent");
                    return OperationResult<bool>.Fail("Could not find foundEvent", "Application");
                }

                if(foundUser.Id != foundEvent.CreatedBy &&
                    foundUser.Role.ToLower() == "superadmin" &&
                    foundUser.Role.ToLower() == "admin")
                {
                    _logger.LogError("User does not have permission to delete this event");
                    return OperationResult<bool>.Fail("User does not have permission to delete this event", "Application");
                }

                var eventDeletion = await _eventRepository.DeleteAsync(foundEvent);
                _logger.LogInformation("Event deleted successfully");

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: " + ex.Message);
                return OperationResult<bool>.Fail("Unexpected error", "Application");
            }
        }
    }
}
