using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.DeleteEvent
{
    public class DeleteEventCommandHandler(IImageHandler imageHandler, IPermissionChecker permissionChecker, UserManager<User> userManager, IGenericRepository<Event> eventRepository, ILogger<DeleteEventCommandHandler> logger) : IRequestHandler<DeleteEventCommand, OperationResult<bool>>
    {
    
        public async Task<OperationResult<bool>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.EventId == string.Empty)
                {
                    logger.LogError("Id is null");
                    return OperationResult<bool>.Fail("EventId can't be null", "Application");
                }

                var foundUser = await userManager.FindByIdAsync(request.UserId);
                if (foundUser == null)
                {
                    logger.LogError("User not found");
                    return OperationResult<bool>.Fail("User not found", "Application");
                }

                var foundEvent = await eventRepository.GetByIdAsync(request.EventId);
                if (foundEvent == null)
                {
                    logger.LogError("Could not find foundEvent");
                    return OperationResult<bool>.Fail("Could not find foundEvent", "Application");
                }

                if (!permissionChecker.HasPermissionToModifyAsync(foundUser, foundEvent))
                {
                    return OperationResult<bool>.Fail("User does not have permission to delete this event", "Application");
                }

                if(! string.IsNullOrWhiteSpace(foundEvent.ImageUrl))
                {
                    var imageDeletion = await imageHandler.DeleteImageAsync(foundEvent.ImageUrl);
                    if (!imageDeletion.Succeeded)
                    {
                        logger.LogError("Could not delete image: " + imageDeletion.ErrorMessage);
                        return OperationResult<bool>.Fail("Could not delete image", "Application");
                    }
                }

                var eventDeletion = await eventRepository.DeleteAsync(foundEvent);
                logger.LogInformation("Event deleted successfully");

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                logger.LogError("Unexpected error: " + ex.Message);
                return OperationResult<bool>.Fail("Unexpected error", "Application");
            }
        }
    }
}
