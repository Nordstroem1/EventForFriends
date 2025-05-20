using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.CreateEvent
{
    public class CreateEventCommandHandler(IImageHandler imageHandler, UserManager<User> userManager, ILogger<CreateEventCommandHandler> logger, IMapper mapper, IGenericRepository<Event> eventRepository) : IRequestHandler<CreateEventCommand, OperationResult<Event>>
    {
        public async Task<OperationResult<Event>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.EventDto == null)
                {
                    logger.LogError("Event data is required");
                    return OperationResult<Event>.Fail("Event data is required", "CreateEventCommandHandler");
                }

                var foundUser = await userManager.FindByIdAsync(request.UserId);

                if (foundUser == null)
                {
                    logger.LogError("User not found");
                    return OperationResult<Event>.Fail("User not found", "CreateEventCommandHandler");
                }

                var imageUploadResponse = await imageHandler.UploadImageAsync(request.ImageFile, "Event");

                if(imageUploadResponse == null || imageUploadResponse.ErrorMessage.Length > 0)
                {
                    logger.LogError("Image upload failed");
                    return OperationResult<Event>.Fail("Image upload failed", "CreateEventCommandHandler");
                }

                var newEvent = mapper.Map<Event>(request);
                newEvent.ImageUrl = imageUploadResponse.Data;

                if (request.ImageFile == null)
                {
                    newEvent.ImageUrl = "no image";
                }
                newEvent.CreatedBy = foundUser.Id;

                await eventRepository.AddAsync(newEvent);
                logger.LogInformation("Event created successfully");

                return OperationResult<Event>.Success(newEvent);
            }
            catch
            {
                return OperationResult<Event>.Fail("Unexpected error", "CreateEventCommandHandler");
            }
        }
    }
}
