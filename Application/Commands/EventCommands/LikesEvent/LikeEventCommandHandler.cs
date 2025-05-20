using Application.Commands.EventCommands.LikesEvent;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.LikeEvent
{
    public class LikeEventCommandHandler(IMySqlContext mySqlDb, 
                                         IGenericRepository<Event> eventRepository, 
                                         UserManager<User> userRepository, 
                                         ILogger<LikeEventCommandHandler> logger) : IRequestHandler<LikeEventCommand, OperationResult<int>>
    {
        private readonly IGenericRepository<Event> _eventRepository = eventRepository;
        private readonly UserManager<User> _userManager = userRepository;
        private readonly ILogger<LikeEventCommandHandler> _logger = logger;
        private readonly IMySqlContext _mySqlDb = mySqlDb;

        public async Task<OperationResult<int>> Handle(LikeEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await _eventRepository.GetByIdAsync(request.EventId);
            if (eventEntity == null)
            {
                _logger.LogError("Event not found.");
                return OperationResult<int>.Fail("Event not found", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.UserId);

            if (foundUser == null)
            {
                _logger.LogError("User not found.");
                return OperationResult<int>.Fail("User not found", "Application");
            }

            var userLikeResult = await RemoveLikeIfAlreadyLiked(eventEntity, foundUser);
            if (userLikeResult != null)
            {
                return userLikeResult;
            }

            eventEntity.LikeList.Add(foundUser);
            await _eventRepository.UpdateAsync(eventEntity);

            _logger.LogInformation("Event liked successfully.");

            return OperationResult<int>.Success(eventEntity.LikeList.Count);
        }

        private async Task<OperationResult<int>> RemoveLikeIfAlreadyLiked(Event eventEntity, User? foundUser)
        {
            await _mySqlDb.LoadCollectionAsync(eventEntity, nameof(eventEntity.LikeList));

            var trackedUser = eventEntity.LikeList.FirstOrDefault(u => u.Id == foundUser.Id);

            if (trackedUser != null)
            {
                eventEntity.LikeList.Remove(trackedUser);

                await _eventRepository.UpdateAsync(eventEntity);
                _logger.LogInformation("User has removed like.");

                return OperationResult<int>.Success(eventEntity.LikeList.Count);
            }

            return null;
        }
    }
}
