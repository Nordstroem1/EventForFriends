using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.EventCommands.CreateEvent
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, OperationResult<Event>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateEventCommandHandler> _logger;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly IMapper _mapper;
        public CreateEventCommandHandler(UserManager<User> userManager, ILogger<CreateEventCommandHandler> logger, IMapper mapper, IGenericRepository<Event> eventRepository)
        {
            _userManager = userManager;
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
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

                var newEvent = _mapper.Map<Event>(request.EventDto);
                newEvent.CreatedBy = foundUser.Id;

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
