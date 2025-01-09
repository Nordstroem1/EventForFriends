using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.LikeEvent
{
    public class LikeEventCommandHandler : IRequestHandler<LikeEventCommand, OperationResult<int>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LikeEventCommandHandler> _logger;

        public LikeEventCommandHandler(IGenericRepository<Event> eventRepository, UserManager<User> userRepository, ILogger<LikeEventCommandHandler> logger)
        {
            _eventRepository = eventRepository;
            _userManager = userRepository;
            _logger = logger;
        }
        public async Task<OperationResult<int>> Handle(LikeEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.LikeEventDto.EventId);
            if (eventEntity == null)
            {
                _logger.LogError("Event not found.");
                return OperationResult<int>.Fail("Event not found", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.LikeEventDto.UserId.ToString());

            if (foundUser == null)
            {
                _logger.LogError("User not found.");
                return OperationResult<int>.Fail("User not found", "Application");
            }

            var userLikeResult = await RemoveLikeIfAlreadyLiked(request, eventEntity, foundUser);
            if (userLikeResult != null)
            {
                return userLikeResult;
            }

            eventEntity.LikeList.Add(foundUser);
            await _eventRepository.UpdateAsync(eventEntity);

            _logger.LogInformation("Event liked successfully.");

            return OperationResult<int>.Success(eventEntity.LikeList.Count);
        }

        private async Task<OperationResult<int>> RemoveLikeIfAlreadyLiked(LikeEventCommand request, Event eventEntity, User? foundUser)
        {
            if (eventEntity.LikeList.Any(u => u.Id == request.LikeEventDto.UserId))
            {
                eventEntity.LikeList.Remove(foundUser);
                await _eventRepository.UpdateAsync(eventEntity);

                _logger.LogInformation("User have removed like.");

                return OperationResult<int>.Success(eventEntity.LikeList.Count);
            }

            return null;
        }
    }
}
